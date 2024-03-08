using MediatR;
using ServiceModels;

namespace SmartFleets.Application.Notifications;

public sealed record TirePressureNotification(TirePressure TirePressure) : INotification;
