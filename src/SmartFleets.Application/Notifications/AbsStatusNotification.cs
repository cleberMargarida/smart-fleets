using MediatR;
using ServiceModels;

namespace SmartFleets.Application.Notifications;

public sealed record AbsStatusNotification(AbsStatus AbsStatus) : INotification;
