using MessagePack;

namespace ServiceModels;

[GenerateSerializer]
[MessagePackObject(keyAsPropertyName: true)]
public partial class VehicleState
{
    [Id(0)]
    public AbsStatus? AbsStatus { get; set; }
    [Id(1)]
    public BatteryVoltage? BatteryVoltage { get; set; }
    [Id(2)]
    public BrakePedalStatus? BrakePedalStatus { get; set; }
    [Id(3)]
    public DoorOpenStatus? DoorOpenStatus { get; set; }
    [Id(4)]
    public EngineRpm? EngineRpm { get; set; }
    [Id(5)]
    public FuelLevel? FuelLevel { get; set; }
    [Id(6)]
    public Gps? Gps { get; set; }
    [Id(7)]
    public IgnitionStatus? IgnitionStatus { get; set; }
    [Id(8)]
    public Odometer? Odometer { get; set; }
    [Id(9)]
    public Speed? Speed { get; set; }
    [Id(10)]
    public Temperature? Temperature { get; set; }
    [Id(11)]
    public TirePressure? TirePressure { get; set; }
}
