using MediatR;
using ServiceModels;

namespace SmartFleets.Application.Notifications;

public sealed record OdometerNotification(Odometer Odometer) : INotification;
