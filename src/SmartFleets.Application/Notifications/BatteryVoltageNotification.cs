using MediatR;
using ServiceModels;

namespace SmartFleets.Application.Notifications;

public sealed record BatteryVoltageNotification(BatteryVoltage BatteryVoltage) : INotification;
