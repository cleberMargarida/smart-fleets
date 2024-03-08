using MediatR;
using SmartFleets.Application.Notifications;

namespace SmartFleets.Application.Handlers;

public class FaultHandler : INotificationHandler<VehicleHistoricalStateNotification>
{
    public Task Handle(VehicleHistoricalStateNotification notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

