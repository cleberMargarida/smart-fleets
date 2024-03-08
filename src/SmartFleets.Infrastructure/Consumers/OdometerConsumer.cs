using MediatR;
using ServiceModels;
using SmartFleets.Application.Notifications;
using SmartFleets.RabbitMQ.Messaging;

namespace SmartFleets.Infrastructure.Consumers;

public class OdometerConsumer : IConsumer<Odometer>
{
    private readonly IMediator _mediator;

    public OdometerConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public Task ConsumeAsync(Odometer signal)
    {
        return _mediator.Publish(new OdometerNotification(signal));
    }
}

