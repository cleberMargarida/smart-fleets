using MediatR;
using ServiceModels;
using SmartFleets.Application.Notifications;
using SmartFleets.RabbitMQ.Messaging;

namespace SmartFleets.Infrastructure.Consumers;

public class VehicleStateConsumer : IConsumer<VehicleState>
{
    private readonly IMediator _mediator;

    public VehicleStateConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public Task ConsumeAsync(VehicleState signal)
    {
        return _mediator.Publish(new VehicleStateNotification(signal));
    }
}
