namespace ServiceModels.Binding;

/// <summary>
/// Enumerates the types of signals that can be associated with a vehicle.
/// </summary>
public enum SignalType : uint
{
    /// <summary>
    /// The speed of the vehicle.
    /// </summary>
    Speed = 1,

    /// <summary>
    /// The odometer reading of the vehicle.
    /// </summary>
    Odometer = 2,

    /// <summary>
    /// The GPS coordinates of the vehicle.
    /// </summary>
    Gps = 3,

    /// <summary>
    /// The engine RPM of the vehicle.
    /// </summary>
    EngineRpm = 4,

    /// <summary>
    /// The fuel level of the vehicle.
    /// </summary>
    FuelLevel = 5,

    /// <summary>
    /// The battery voltage of the vehicle.
    /// </summary>
    BatteryVoltage = 6,

    /// <summary>
    /// The brake pedal status of the vehicle.
    /// </summary>
    BrakePedalStatus = 7,

    /// <summary>
    /// The ABS status of the vehicle.
    /// </summary>
    AbsStatus = 8,

    /// <summary>
    /// The tire pressure of the vehicle.
    /// </summary>
    TirePressure = 9,

    /// <summary>
    /// The door open status of the vehicle.
    /// </summary>
    DoorOpenStatus = 10,

    /// <summary>
    /// The ignition status of the vehicle.
    /// </summary>
    IgnitionStatus = 11,

    /// <summary>
    /// The temperature inside the vehicle.
    /// </summary>
    Temperature = 12
}
