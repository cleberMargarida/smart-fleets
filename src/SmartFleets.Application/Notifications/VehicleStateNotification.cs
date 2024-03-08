using MediatR;
using ServiceModels;

namespace SmartFleets.Application.Notifications;

public sealed record VehicleStateNotification(VehicleState VehicleState) : INotification;
