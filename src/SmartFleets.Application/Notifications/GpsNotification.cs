using MediatR;
using ServiceModels;

namespace SmartFleets.Application.Notifications;

public sealed record GpsNotification(Gps Gps) : INotification;
