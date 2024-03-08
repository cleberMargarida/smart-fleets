using MediatR;
using ServiceModels;
using SmartFleets.Application.Notifications;
using SmartFleets.RabbitMQ.Messaging;

namespace SmartFleets.Infrastructure.Consumers;

public class FuelLevelConsumer : IConsumer<FuelLevel>
{
    private readonly IMediator _mediator;

    public FuelLevelConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public Task ConsumeAsync(FuelLevel signal)
    {
        return _mediator.Publish(new FuelLevelNotification(signal));
    }
}

