using MessagePack;
using ServiceModels.Binding;
using SmartFleets.RabbitMQ.Base;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace ServiceModels.Abstractions;

/// <summary>
/// Represents the base class for all signal types.
/// </summary>
[GenerateSerializer]
public abstract class BaseSignal : IRoutingKeyProviderableObject
{
    [Id(0)]
    private SignalType? _signalType;

    /// <summary>
    /// Gets or sets the unique identifier of the signal.
    /// </summary>
    [Id(1)]
    public required string Id { get; set; }

    /// <summary>
    /// Gets or sets the tenant identifier associated with the signal.
    /// </summary>
    [Id(2)]
    public required int TenantId { get; set; }

    /// <summary>
    /// Gets or sets the vehicle identifier associated with the signal.
    /// </summary>
    [Id(3)]
    public required string VehicleId { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the signal was generated, in UTC.
    /// </summary>
    [Id(4)]
    public required DateTime DateTimeUtc { get; set; }

    /// <summary>
    /// Determines whether this signal is newer than another signal.
    /// </summary>
    /// <param name="compare">The signal to compare with.</param>
    /// <returns>True if this signal is newer, otherwise false.</returns>
    public bool IsNewerThan(BaseSignal? compare)
    {
        return compare == null || DateTimeUtc > compare.DateTimeUtc;
    }

    /// <summary>
    /// Determines whether this signal is older than another signal.
    /// </summary>
    /// <param name="compare">The signal to compare with.</param>
    /// <returns>True if this signal is older, otherwise false.</returns>
    public bool IsOlderThan(BaseSignal? compare)
    {
        return compare != null && DateTimeUtc < compare.DateTimeUtc;
    }

    /// <summary>
    /// Generates a routing key for this signal.
    /// </summary>
    /// <returns>The routing key.</returns>
    string IRoutingKeyProviderableObject.ToRoutingKey()
    {
        return VehicleId + ".#";
    }

    /// <summary>
    /// Gets the type of the signal.
    /// </summary>
    [NotMapped, IgnoreMember]
    public SignalType Type
    {
        get => _signalType ??= GetType().GetCustomAttribute<TypeIdAttribute>()?.Type
            ?? throw TypeIdNotDecoratedException.NotDecorated(GetType());
    }

    /// <summary>
    /// Gets or sets the date and time when the signal was ingested into the system.
    /// </summary>
    /// <remarks>
    /// This property is not serialized.
    /// </remarks>
    [field: NonSerialized, NotMapped, IgnoreMember]
    public DateTime IngestedDateTimeUtc { get; set; }
}
