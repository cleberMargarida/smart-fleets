using MessagePack;
using ServiceModels.Abstractions;
using ServiceModels.Binding;

namespace ServiceModels;

[TypeId(SignalType.AbsStatus), MessagePackObject(keyAsPropertyName: true)]
public sealed class AbsStatus : BooleanValue { }

[TypeId(SignalType.BatteryVoltage), MessagePackObject(keyAsPropertyName: true)]
public sealed class BatteryVoltage : Numeric { }

[TypeId(SignalType.BrakePedalStatus), MessagePackObject(keyAsPropertyName: true)]
public sealed class BrakePedalStatus : Numeric { }

[TypeId(SignalType.DoorOpenStatus), MessagePackObject(keyAsPropertyName: true)]
public sealed class DoorOpenStatus : BooleanValue { }

[TypeId(SignalType.EngineRpm), MessagePackObject(keyAsPropertyName: true)]
public sealed class EngineRpm : Numeric { }

[TypeId(SignalType.FuelLevel), MessagePackObject(keyAsPropertyName: true)]
public sealed class FuelLevel : Numeric { }

[TypeId(SignalType.Gps), MessagePackObject(keyAsPropertyName: true)]
public sealed class Gps : Coordinate { }

[TypeId(SignalType.IgnitionStatus), MessagePackObject(keyAsPropertyName: true)]
public sealed class IgnitionStatus : BooleanValue { }

[TypeId(SignalType.Odometer), MessagePackObject(keyAsPropertyName: true)]
public sealed class Odometer : Numeric { }

[TypeId(SignalType.Speed), MessagePackObject(keyAsPropertyName: true)]
public sealed class Speed : Numeric { }

[TypeId(SignalType.Temperature), MessagePackObject(keyAsPropertyName: true)]
public sealed class Temperature : Numeric { }

[TypeId(SignalType.TirePressure), MessagePackObject(keyAsPropertyName: true)]
public sealed class TirePressure : Numeric { }
