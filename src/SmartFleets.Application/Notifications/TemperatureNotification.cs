using MediatR;
using ServiceModels;

namespace SmartFleets.Application.Notifications;

public sealed record TemperatureNotification(Temperature Temperature) : INotification;
