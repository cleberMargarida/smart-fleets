using MediatR;
using ServiceModels;
using SmartFleets.Application.Notifications;
using SmartFleets.RabbitMQ.Messaging;

namespace SmartFleets.Infrastructure.Consumers;

public class VehicleHistoricalStateConsumer : IConsumer<VehicleHistoricalState>
{
    private readonly IMediator _mediator;

    public VehicleHistoricalStateConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public Task ConsumeAsync(VehicleHistoricalState signal)
    {
        return _mediator.Publish(new VehicleHistoricalStateNotification(signal));
    }
}
