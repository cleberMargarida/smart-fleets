using MediatR;
using ServiceModels;
using SmartFleets.Application.Notifications;
using SmartFleets.RabbitMQ.Messaging;

namespace SmartFleets.Infrastructure.Consumers;

public class EngineRpmConsumer : IConsumer<EngineRpm>
{
    private readonly IMediator _mediator;

    public EngineRpmConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public Task ConsumeAsync(EngineRpm signal)
    {
        return _mediator.Publish(new EngineRpmNotification(signal));
    }
}

