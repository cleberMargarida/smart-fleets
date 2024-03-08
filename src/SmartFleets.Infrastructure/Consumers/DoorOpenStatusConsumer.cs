using MediatR;
using ServiceModels;
using SmartFleets.Application.Notifications;
using SmartFleets.RabbitMQ.Messaging;
namespace SmartFleets.Infrastructure.Consumers;

public class DoorOpenStatusConsumer : IConsumer<DoorOpenStatus>
{
    private readonly IMediator _mediator;

    public DoorOpenStatusConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public Task ConsumeAsync(DoorOpenStatus signal)
    {
        return _mediator.Publish(new DoorOpenStatusNotification(signal));
    }
}

