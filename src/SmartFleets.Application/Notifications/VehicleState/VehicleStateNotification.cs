using MediatR;

namespace SmartFleets.Application.Notifications.VehicleState
{
    using ServiceModels;

    /// <summary>
    /// Represents a notification containing the state of a vehicle.
    /// </summary>
    /// <param name="VehicleState">The vehicle state.</param>
    public record VehicleStateNotification(VehicleState VehicleState) : INotification;
}
