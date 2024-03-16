using Mapster;
using MediatR;
using SmartFleets.Application.Commands.CreateFault;
using SmartFleets.Application.Notifications.VehicleState;
using SmartFleets.Application.Queries.GetFaultMetadata;
using SmartFleets.Domain.Entities;
using SmartFleets.Domain.Repositories;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace SmartFleets.Application.Notifications.SimulateFaults
{
    /// <summary>
    /// Represents a handler for simulating faults based on vehicle state notifications.
    /// </summary>
    public sealed class SimulateFaultsCommandHandler : INotificationHandler<VehicleStateNotification>
    {
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="SimulateFaultsCommandHandler"/> class.
        /// </summary>
        /// <param name="mediator">The mediator for sending commands and queries.</param>
        /// <param name="unitOfWork">The unit of work for managing database transactions.</param>
        public SimulateFaultsCommandHandler(IMediator mediator, IUnitOfWork unitOfWork)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Handles the simulation of faults based on the vehicle state notification.
        /// </summary>
        /// <param name="request">The vehicle state notification.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task Handle(VehicleStateNotification request, CancellationToken cancellationToken)
        {
            var latestSignal = request.VehicleState.Last();
            var query = latestSignal.Adapt<GetFaultMetadataBySignalTypeQuery>();
            var faultMetadatas = await _mediator.Send(query, cancellationToken);

            foreach (var faultMetadata in faultMetadatas)
            {
                if (!faultMetadata.Simulate(request.VehicleState))
                {
                    continue;
                }

                var fault = new Fault
                {
                    CreatedAt = latestSignal.DateTimeUtc,
                    Description = faultMetadata.Description,
                    SignalId = latestSignal.Id,
                    SignalTypes = faultMetadata.SignalTypes,
                    VehicleId = latestSignal.VehicleId,
                };

                var createFaultCommand = fault.Adapt<CreateFaultCommand>();
                await _mediator.Send(createFaultCommand, cancellationToken);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
