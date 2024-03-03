using MessagePack;
using ServiceModels.Abstractions;
using ServiceModels.Binding;

namespace ServiceModels;

[TypeId(SignalType.AbsStatus)]
[MessagePackObject(keyAsPropertyName: true)]
[GenerateSerializer]
public sealed class AbsStatus : BooleanValue { }

[TypeId(SignalType.BatteryVoltage)]
[MessagePackObject(keyAsPropertyName: true)]
[GenerateSerializer]
public sealed class BatteryVoltage : Numeric { }

[TypeId(SignalType.BrakePedalStatus)]
[MessagePackObject(keyAsPropertyName: true)]
[GenerateSerializer]
public sealed class BrakePedalStatus : Numeric { }

[TypeId(SignalType.DoorOpenStatus)]
[MessagePackObject(keyAsPropertyName: true)]
[GenerateSerializer]
public sealed class DoorOpenStatus : BooleanValue { }

[TypeId(SignalType.EngineRpm)]
[MessagePackObject(keyAsPropertyName: true)]
[GenerateSerializer]
public sealed class EngineRpm : Numeric { }

[TypeId(SignalType.FuelLevel)]
[MessagePackObject(keyAsPropertyName: true)]
[GenerateSerializer]
public sealed class FuelLevel : Numeric { }

[TypeId(SignalType.Gps)]
[MessagePackObject(keyAsPropertyName: true)]
[GenerateSerializer]
public sealed class Gps : Coordinate { }

[TypeId(SignalType.IgnitionStatus)]
[MessagePackObject(keyAsPropertyName: true)]
[GenerateSerializer]
public sealed class IgnitionStatus : BooleanValue { }

[TypeId(SignalType.Odometer)]
[MessagePackObject(keyAsPropertyName: true)]
[GenerateSerializer]
public sealed class Odometer : Numeric { }

[TypeId(SignalType.Speed)]
[MessagePackObject(keyAsPropertyName: true)]
[GenerateSerializer]
public sealed class Speed : Numeric { }

[TypeId(SignalType.Temperature)]
[MessagePackObject(keyAsPropertyName: true)]
[GenerateSerializer]
public sealed class Temperature : Numeric { }

[TypeId(SignalType.TirePressure)]
[MessagePackObject(keyAsPropertyName: true)]
[GenerateSerializer]
public sealed class TirePressure : Numeric { }
