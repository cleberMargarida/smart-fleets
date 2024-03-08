using MediatR;
using ServiceModels;
using SmartFleets.Application.Notifications;
using SmartFleets.RabbitMQ.Messaging;

namespace SmartFleets.Infrastructure.Consumers;

public class TirePressureConsumer : IConsumer<TirePressure>
{
    private readonly IMediator _mediator;

    public TirePressureConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public Task ConsumeAsync(TirePressure signal)
    {
        return _mediator.Publish(new TirePressureNotification(signal));
    }
}

