using MediatR;
using ServiceModels;
using SmartFleets.Application.Notifications;
using SmartFleets.RabbitMQ.Messaging;

namespace SmartFleets.Infrastructure.Consumers;

public class BrakePedalStatusConsumer : IConsumer<BrakePedalStatus>
{
    private readonly IMediator _mediator;

    public BrakePedalStatusConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public Task ConsumeAsync(BrakePedalStatus signal)
    {
        return _mediator.Publish(new BrakePedalStatusNotification(signal));
    }
}

