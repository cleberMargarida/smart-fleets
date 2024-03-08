using MediatR;
using ServiceModels;
using SmartFleets.Application.Notifications;
using SmartFleets.RabbitMQ.Messaging;

namespace SmartFleets.Infrastructure.Consumers;

public class TemperatureConsumer : IConsumer<Temperature>
{
    private readonly IMediator _mediator;

    public TemperatureConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public Task ConsumeAsync(Temperature signal)
    {
        return _mediator.Publish(new TemperatureNotification(signal));
    }
}

