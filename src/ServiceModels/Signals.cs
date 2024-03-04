using MessagePack;
using ServiceModels.Abstractions;
using ServiceModels.Binding;

namespace ServiceModels;

/// <summary>
/// Represents the ABS status of a vehicle as a boolean value.
/// </summary>
[TypeId(SignalType.AbsStatus)]
[MessagePackObject(keyAsPropertyName: true)]
[GenerateSerializer]
public sealed class AbsStatus : BooleanValue { }

/// <summary>
/// Represents the battery voltage of a vehicle as a numeric value.
/// </summary>
[TypeId(SignalType.BatteryVoltage)]
[MessagePackObject(keyAsPropertyName: true)]
[GenerateSerializer]
public sealed class BatteryVoltage : Numeric { }

/// <summary>
/// Represents the brake pedal status of a vehicle as a numeric value.
/// </summary>
[TypeId(SignalType.BrakePedalStatus)]
[MessagePackObject(keyAsPropertyName: true)]
[GenerateSerializer]
public sealed class BrakePedalStatus : Numeric { }

/// <summary>
/// Represents the door open status of a vehicle as a boolean value.
/// </summary>
[TypeId(SignalType.DoorOpenStatus)]
[MessagePackObject(keyAsPropertyName: true)]
[GenerateSerializer]
public sealed class DoorOpenStatus : BooleanValue { }

/// <summary>
/// Represents the engine RPM of a vehicle as a numeric value.
/// </summary>
[TypeId(SignalType.EngineRpm)]
[MessagePackObject(keyAsPropertyName: true)]
[GenerateSerializer]
public sealed class EngineRpm : Numeric { }

/// <summary>
/// Represents the fuel level of a vehicle as a numeric value.
/// </summary>
[TypeId(SignalType.FuelLevel)]
[MessagePackObject(keyAsPropertyName: true)]
[GenerateSerializer]
public sealed class FuelLevel : Numeric { }

/// <summary>
/// Represents the GPS coordinates of a vehicle.
/// </summary>
[TypeId(SignalType.Gps)]
[MessagePackObject(keyAsPropertyName: true)]
[GenerateSerializer]
public sealed class Gps : Coordinate { }

/// <summary>
/// Represents the ignition status of a vehicle as a boolean value.
/// </summary>
[TypeId(SignalType.IgnitionStatus)]
[MessagePackObject(keyAsPropertyName: true)]
[GenerateSerializer]
public sealed class IgnitionStatus : BooleanValue { }

/// <summary>
/// Represents the odometer reading of a vehicle as a numeric value.
/// </summary>
[TypeId(SignalType.Odometer)]
[MessagePackObject(keyAsPropertyName: true)]
[GenerateSerializer]
public sealed class Odometer : Numeric { }

/// <summary>
/// Represents the speed of a vehicle as a numeric value.
/// </summary>
[TypeId(SignalType.Speed)]
[MessagePackObject(keyAsPropertyName: true)]
[GenerateSerializer]
public sealed class Speed : Numeric { }

/// <summary>
/// Represents the temperature of a vehicle as a numeric value.
/// </summary>
[TypeId(SignalType.Temperature)]
[MessagePackObject(keyAsPropertyName: true)]
[GenerateSerializer]
public sealed class Temperature : Numeric { }

/// <summary>
/// Represents the tire pressure of a vehicle as a numeric value.
/// </summary>
[TypeId(SignalType.TirePressure)]
[MessagePackObject(keyAsPropertyName: true)]
[GenerateSerializer]
public sealed class TirePressure : Numeric { }
