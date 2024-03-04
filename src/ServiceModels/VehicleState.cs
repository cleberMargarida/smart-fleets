using MessagePack;

namespace ServiceModels;

/// <summary>
/// Represents the state of a vehicle.
/// </summary>
[GenerateSerializer]
[MessagePackObject(keyAsPropertyName: true)]
public partial class VehicleState
{
    /// <summary>
    /// Gets or sets the ABS status of the vehicle.
    /// </summary>
    [Id(0)]
    public AbsStatus? AbsStatus { get; set; }

    /// <summary>
    /// Gets or sets the battery voltage of the vehicle.
    /// </summary>
    [Id(1)]
    public BatteryVoltage? BatteryVoltage { get; set; }

    /// <summary>
    /// Gets or sets the brake pedal status of the vehicle.
    /// </summary>
    [Id(2)]
    public BrakePedalStatus? BrakePedalStatus { get; set; }

    /// <summary>
    /// Gets or sets the door open status of the vehicle.
    /// </summary>
    [Id(3)]
    public DoorOpenStatus? DoorOpenStatus { get; set; }

    /// <summary>
    /// Gets or sets the engine RPM of the vehicle.
    /// </summary>
    [Id(4)]
    public EngineRpm? EngineRpm { get; set; }

    /// <summary>
    /// Gets or sets the fuel level of the vehicle.
    /// </summary>
    [Id(5)]
    public FuelLevel? FuelLevel { get; set; }

    /// <summary>
    /// Gets or sets the GPS location of the vehicle.
    /// </summary>
    [Id(6)]
    public Gps? Gps { get; set; }

    /// <summary>
    /// Gets or sets the ignition status of the vehicle.
    /// </summary>
    [Id(7)]
    public IgnitionStatus? IgnitionStatus { get; set; }

    /// <summary>
    /// Gets or sets the odometer reading of the vehicle.
    /// </summary>
    [Id(8)]
    public Odometer? Odometer { get; set; }

    /// <summary>
    /// Gets or sets the speed of the vehicle.
    /// </summary>
    [Id(9)]
    public Speed? Speed { get; set; }

    /// <summary>
    /// Gets or sets the temperature inside the vehicle.
    /// </summary>
    [Id(10)]
    public Temperature? Temperature { get; set; }

    /// <summary>
    /// Gets or sets the tire pressure of the vehicle.
    /// </summary>
    [Id(11)]
    public TirePressure? TirePressure { get; set; }
}
