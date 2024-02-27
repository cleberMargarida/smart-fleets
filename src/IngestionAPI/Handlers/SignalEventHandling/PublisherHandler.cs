using AutoMapper;
using IngestionAPI.Handlers.SignalEventHandling;
using IngestionAPI.Models;
using SmartFleet.RabbitMQ.Base;

namespace Ingestion.Api.Handlers.SignalEventHandling;

public class PublisherHandler : ISignalHandler<Signal>
{
    private readonly IBus _bus;
    private readonly IMapper _mapper;

    public PublisherHandler(IBus bus, IMapper mapper)
    {
        _bus = bus;
        _mapper = mapper;
    }

    public async Task HandleAsync(Signal signal)
    {
        var message = _mapper.Map(
            signal, 
            signal.GetType(), 
            signal.DestinationType());

        await _bus.PublishAsync(message);
    }
}
