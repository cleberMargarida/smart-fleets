using MediatR;
using ServiceModels;
using SmartFleets.Application.Notifications;
using SmartFleets.RabbitMQ.Messaging;

namespace SmartFleets.Infrastructure.Consumers;

public class SpeedConsumer : IConsumer<Speed>
{
    private readonly IMediator _mediator;

    public SpeedConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public Task ConsumeAsync(Speed signal)
    {
        return _mediator.Publish(new SpeedNotification(signal));
    }
}

