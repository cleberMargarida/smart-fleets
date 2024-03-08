using MediatR;
using ServiceModels;

namespace SmartFleets.Application.Notifications;

public sealed record FuelLevelNotification(FuelLevel FuelLevel) : INotification;
