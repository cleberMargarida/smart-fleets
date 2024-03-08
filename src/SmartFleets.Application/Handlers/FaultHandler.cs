using MediatR;
using SmartFleets.Application.Notifications;

namespace SmartFleets.Application.Handlers;

public class FaultHandler : INotificationHandler<VehicleStateNotification>
{
    public Task Handle(VehicleStateNotification notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

