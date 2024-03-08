using MediatR;
using ServiceModels;

namespace SmartFleets.Application.Notifications;

public sealed record IgnitionStatusNotification(IgnitionStatus IgnitionStatus) : INotification;
