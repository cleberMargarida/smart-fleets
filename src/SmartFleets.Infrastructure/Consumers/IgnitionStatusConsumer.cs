using MediatR;
using ServiceModels;
using SmartFleets.Application.Notifications;
using SmartFleets.RabbitMQ.Messaging;

namespace SmartFleets.Infrastructure.Consumers;

public class IgnitionStatusConsumer : IConsumer<IgnitionStatus>
{
    private readonly IMediator _mediator;

    public IgnitionStatusConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public Task ConsumeAsync(IgnitionStatus signal)
    {
        return _mediator.Publish(new IgnitionStatusNotification(signal));
    }
}

