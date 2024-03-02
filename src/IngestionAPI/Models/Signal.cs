using MessagePack;
using SmartFleets.RabbitMQ.Base;

namespace IngestionAPI.Models;

/// <summary>
/// Represents a signal containing data from a vehicle.
/// </summary>
[MessagePackObject(keyAsPropertyName: true)]
public class Signal : IRoutingKeyProviderableObject
{
    /// <summary>
    /// Gets or sets the unique identifier of the signal.
    /// </summary>
    public required string Id { get; set; }

    /// <summary>
    /// Gets or sets the tenant identifier associated with the signal.
    /// </summary>
    public required int TenantId { get; set; }

    /// <summary>
    /// Gets or sets the vehicle identifier associated with the signal.
    /// </summary>
    public required string VehicleId { get; set; }

    /// <summary>
    /// Gets or sets the type of the signal.
    /// </summary>
    public required uint Type { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the signal was generated.
    /// </summary>
    public required DateTime DateTimeUtc { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the signal was ingested into the system.
    /// This property is not serialized.
    /// </summary>
    [IgnoreMember]
    public DateTime IngestedDateTimeUtc { get; set; }

    /// <summary>
    /// Gets or sets the value of the signal, if it is numeric.
    /// </summary>
    public double? Value { get; set; }

    /// <summary>
    /// Gets or sets a dictionary of values, if the signal contains multiple numeric values.
    /// </summary>
    public Dictionary<int, double>? Values { get; set; }

    /// <summary>
    /// Indicates whether the signal has a numeric value.
    /// </summary>
    public bool IsNumeric => Value is not null;

    /// <summary>
    /// Generates a routing key based on the vehicle identifier.
    /// </summary>
    /// <returns>A routing key for the signal.</returns>
    public string ToRoutingKey()
    {
        return VehicleId + ".#";
    }
}
