using MessagePack;
using SmartFleet.RabbitMQ.Messaging;

namespace IngestionAPI.Models;

[MessagePackObject(keyAsPropertyName: true)]
public class Signal : IRoutingKeyProviderableObject
{
    public required string Id { get; set; }
    public required int TenantId { get; set; }
    public required string VehicleId { get; set; }
    public required uint SignalType { get; set; }
    public required DateTime DateTime { get; set; }

    public double? Value { get; set; }
    public Dictionary<int, double>? Values { get; set; }

    public bool IsNumeric => Value is not null;

    public string ToRoutingKey()
    {
        return VehicleId + ".#";
    }
}
