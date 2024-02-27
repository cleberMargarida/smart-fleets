using MessagePack;
using ServiceModels.Binding;
using SmartFleet.RabbitMQ.Messaging;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace ServiceModels.Abstractions;

/// <c>https://github.com/MessagePack-CSharp/MessagePack-CSharp</c>
public abstract class SignalAbstract : IRoutingKeyProviderableObject
{
    private SignalType? _signalType;

    public required string Id { get; set; }
    public required int TenantId { get; set; }
    public required string VehicleId { get; set; }
    public required DateTime DateTime { get; set; }

    public bool IsNewerThan(SignalAbstract? compare)
    {
        return compare == null || DateTime > compare.DateTime;
    }

    public bool IsOlderThan(SignalAbstract? compare)
    {
        return compare != null && DateTime < compare.DateTime;
    }

    string IRoutingKeyProviderableObject.ToRoutingKey()
    {
        return VehicleId + ".#";
    }

    [NotMapped, IgnoreMember]
    public SignalType SignalType
    {
        get => _signalType ??= GetType().GetCustomAttribute<TypeIdAttribute>()?.Type
            ?? throw TypeIdNotDecoratedException.NotDecorated(GetType());
    }
}
