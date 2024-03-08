using MediatR;
using ServiceModels;

namespace SmartFleets.Application.Notifications;

public sealed record SpeedNotification(Speed Speed) : INotification;
