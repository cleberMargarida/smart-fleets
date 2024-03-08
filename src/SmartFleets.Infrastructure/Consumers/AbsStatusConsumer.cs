using MediatR;
using ServiceModels;
using SmartFleets.Application.Notifications;
using SmartFleets.RabbitMQ.Messaging;

namespace SmartFleets.Infrastructure.Consumers;

public class AbsStatusConsumer : IConsumer<AbsStatus>
{
    private readonly IMediator _mediator;

    public AbsStatusConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public Task ConsumeAsync(AbsStatus signal)
    {
        return _mediator.Publish(new AbsStatusNotification(signal));
    }
}

