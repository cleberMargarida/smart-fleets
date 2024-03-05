using AutoMapper;
using Ingestion.Api.Handlers.Abstractions;
using ServiceModels.Abstractions;
using SmartFleets.RabbitMQ.Messaging;

namespace Ingestion.Api.Consumers
{
    /// <summary>
    /// Consumes messages and processes them using a specified pipeline.
    /// </summary>
    public class MessageConsumer : IConsumer<Message>
    {
        private readonly IPipeline _pipeline;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageConsumer"/> class.
        /// </summary>
        /// <param name="pipeline">The pipeline used to execute processing on received messages.</param>
        public MessageConsumer(IPipeline pipeline, IMapper mapper)
        {
            _pipeline = pipeline;
            _mapper = mapper;
        }

        /// <inheritdoc/>
        public async Task ConsumeAsync(Message message) 
        {
            foreach (var signal in message.Signals.Select(ConvertSignal))
            {
                await _pipeline.RunAsync(signal);
            }
        }

        private BaseSignal ConvertSignal(Signal s)
        {
            return (BaseSignal)_mapper.Map(s, s.GetType(), s.DestinationType());
        }
    }
}

