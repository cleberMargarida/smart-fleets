using MessagePack;
using ServiceModels.Binding;
using SmartFleets.RabbitMQ.Base;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace ServiceModels.Abstractions;

[GenerateSerializer]
public abstract class BaseSignal : IRoutingKeyProviderableObject
{
    [Id(0)]
    private SignalType? _signalType;

    [Id(1)]
    public required string Id { get; set; }
    
    [Id(2)]
    public required int TenantId { get; set; }
    
    [Id(3)]
    public required string VehicleId { get; set; }
    
    [Id(4)]
    public required DateTime DateTimeUtc { get; set; }

    public bool IsNewerThan(BaseSignal? compare)
    {
        return compare == null || DateTimeUtc > compare.DateTimeUtc;
    }

    public bool IsOlderThan(BaseSignal? compare)
    {
        return compare != null && DateTimeUtc < compare.DateTimeUtc;
    }

    string IRoutingKeyProviderableObject.ToRoutingKey()
    {
        return VehicleId + ".#";
    }

    [NotMapped, IgnoreMember]
    public SignalType Type
    {
        get => _signalType ??= GetType().GetCustomAttribute<TypeIdAttribute>()?.Type
            ?? throw TypeIdNotDecoratedException.NotDecorated(GetType());
    }

    /// <summary>
    /// Gets or sets the date and time when the signal was ingested into the system.
    /// This property is not serialized.
    /// </summary>
    [field: NonSerialized, NotMapped, IgnoreMember]
    public DateTime IngestedDateTimeUtc { get; set; }
}
