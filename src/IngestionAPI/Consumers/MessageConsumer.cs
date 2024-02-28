using AutoMapper;
using IngestionAPI.Models;
using SmartFleet.RabbitMQ.Base;
using SmartFleet.RabbitMQ.Messaging;

namespace IngestionAPI.Consumers
{
    public class MessageConsumer : IConsumer<Message>
    {
        private readonly IBus _bus;
        private readonly IMapper _mapper;

        public MessageConsumer(IBus bus, IMapper mapper)
        {
            _bus = bus;
            _mapper = mapper;
        }

        public Task ConsumeAsync(Message message) => Task.WhenAll(message
                .Signals
                .Select(signal =>
                {
                    var signalConverted = _mapper.Map(
                        signal,
                        signal.GetType(),
                        signal.DestinationType());

                    return _bus.PublishAsync(signalConverted);
                }));
    }
}
