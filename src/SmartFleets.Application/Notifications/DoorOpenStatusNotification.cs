using MediatR;
using ServiceModels;

namespace SmartFleets.Application.Notifications;

public sealed record DoorOpenStatusNotification(DoorOpenStatus DoorOpenStatus) : INotification;
