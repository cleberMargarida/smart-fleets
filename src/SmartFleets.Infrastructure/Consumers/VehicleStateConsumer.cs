using MediatR;
using ServiceModels;
using SmartFleets.Application.Notifications.VehicleState;
using SmartFleets.RabbitMQ.Messaging;
using System.Threading.Tasks;

namespace SmartFleets.Infrastructure.Consumers
{
    /// <summary>
    /// Represents a consumer that processes vehicle state messages.
    /// </summary>
    public class VehicleStateConsumer : IConsumer<VehicleState>
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleStateConsumer"/> class.
        /// </summary>
        /// <param name="mediator">The mediator for publishing notifications.</param>
        public VehicleStateConsumer(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Consumes a vehicle state message and publishes a corresponding notification.
        /// </summary>
        /// <param name="signal">The vehicle state message.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task ConsumeAsync(VehicleState signal)
        {
            return _mediator.Publish(new VehicleStateNotification(signal));
        }
    }
}
