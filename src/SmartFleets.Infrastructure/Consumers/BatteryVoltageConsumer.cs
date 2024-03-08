using MediatR;
using ServiceModels;
using SmartFleets.Application.Notifications;
using SmartFleets.RabbitMQ.Messaging;

namespace SmartFleets.Infrastructure.Consumers;

public class BatteryVoltageConsumer : IConsumer<BatteryVoltage>
{
    private readonly IMediator _mediator;

    public BatteryVoltageConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public Task ConsumeAsync(BatteryVoltage signal)
    {
        return _mediator.Publish(new BatteryVoltageNotification(signal));
    }
}

