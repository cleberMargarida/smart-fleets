using MediatR;
using ServiceModels;

namespace SmartFleets.Application.Notifications;

public sealed record VehicleHistoricalStateNotification(VehicleHistoricalState VehicleHistoricalState) : INotification;
