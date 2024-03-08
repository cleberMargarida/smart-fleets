using MediatR;
using ServiceModels;

namespace SmartFleets.Application.Notifications;

public sealed record BrakePedalStatusNotification(BrakePedalStatus BrakePedalStatus) : INotification;
