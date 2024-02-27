using Azure.Messaging.EventHubs.Processor;
using Azure.Messaging.EventHubs;
using AutoMapper;
using System.Reflection;
using IngestionAPI.Handlers.MessageErrorHandling;
using IngestionAPI.Handlers.SignalEventHandling;
using IngestionAPI.Handlers.MessageEventHandling;
using IngestionAPI.Handlers.SignalEventValidation;
using IngestionAPI.Models;
using IngestionAPI.Handlers.MessageEventValidation;
using IngestionAPI.EventHub.Attributes;

namespace IngestionAPI.EventHub
{
    public class ProcessEventHandlerService : BackgroundService
    {
        private readonly EventProcessorClient _eventProcessorClient;
        private readonly IEnumerable<IMessageHandler<Message>> _messageHandlers;
        private readonly IEnumerable<IMessageValidator<Message>> _messageValidators;
        private readonly IEnumerable<IMessageErrorHandler<Message>> _messageErrorHandlers;
        private readonly IEnumerable<ISignalHandler<Signal>> _signalHandlers;
        private readonly IEnumerable<ISignalValidator<Signal>> _signalValidators;
        private readonly IMapper _mapper;

        public ProcessEventHandlerService(
            EventProcessorClient eventProcessorClient,
            IEnumerable<IMessageHandler<Message>> messageHandlers,
            IEnumerable<IMessageValidator<Message>> messageValidators,
            IEnumerable<IMessageErrorHandler<Message>> messageErrorHandler,
            IEnumerable<ISignalHandler<Signal>> signalHandlers,
            IEnumerable<ISignalValidator<Signal>> signalValidators,
            IMapper mapper)
        {
            _eventProcessorClient = eventProcessorClient;
            _messageHandlers = messageHandlers;
            _messageValidators = messageValidators;
            _messageErrorHandlers = messageErrorHandler;
            _signalHandlers = signalHandlers.OrderBy(ExecutionOrder).ToList();
            _signalValidators = signalValidators;
            _mapper = mapper;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _eventProcessorClient.ProcessEventAsync += ProcessEventHandler;
            _eventProcessorClient.ProcessErrorAsync += ProcessErrorHandler;
            await _eventProcessorClient.StartProcessingAsync(stoppingToken);
            await Task.Delay(Timeout.Infinite, stoppingToken);
            await _eventProcessorClient.StopProcessingAsync(stoppingToken);
        }

        public async Task ProcessEventHandler(ProcessEventArgs args)
        {
            var message = _mapper.Map<Message>(args);
            await ProcessIngestionEventHandler(message);
        }

        public async Task ProcessIngestionEventHandler(Message message)
        {
            if (await MessageIsInvalidAsync(message))
            {
                return;
            }

            await HandleMessageAsync(message);

            foreach (var signal in message.Signals)
            {
                await HandleSignalAsync(signal);
            }
        }

        private async Task<bool> MessageIsInvalidAsync(Message message)
        {
            foreach (var validator in _messageValidators)
            {
                if (await validator.IsValid(message))
                {
                    continue;
                }
                return true;
            }
            return false;
        }

        private async Task HandleMessageAsync(Message message)
        {
            var messageHandling
                = _messageHandlers.Select(h => h.HandleAsync(message));

            await Task.WhenAll(messageHandling);
        }

        private async Task HandleSignalAsync(Signal signal)
        {
            if (await SignalIsInvalidAsync(signal))
            {
                return;
            }

            foreach (var handler in _signalHandlers)
            {
                await handler.HandleAsync(signal);
            }
        }

        private async Task<bool> SignalIsInvalidAsync(Signal signal)
        {
            foreach (var validator in _signalValidators)
            {
                if (await validator.IsValid(signal))
                {
                    continue;
                }
                return true;
            }
            return false;
        }

        private static int? ExecutionOrder(ISignalHandler<Signal> h)
        {
            return h.GetType()
                    .GetCustomAttribute<ExecutionOrderAttribute>()?
                    .Order;
        }

        private Task ProcessErrorHandler(ProcessErrorEventArgs args)
        {
            var errorMessageHandling
                = _messageErrorHandlers.Select(h => h.HandleError(args));

            return Task.WhenAll(errorMessageHandling);
        }
    }
}