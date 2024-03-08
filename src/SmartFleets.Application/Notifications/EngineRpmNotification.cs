using MediatR;
using ServiceModels;

namespace SmartFleets.Application.Notifications;

public sealed record EngineRpmNotification(EngineRpm EngineRpm) : INotification;
