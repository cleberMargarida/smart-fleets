namespace ServiceModels.Binding;

public enum SignalType : uint
{
    Speed = 1,
    Odometer = 2,
    Gps = 3,
    EngineRpm = 4,
    FuelLevel = 5,
    BatteryVoltage = 6,
    BrakePedalStatus = 7,
    AbsStatus = 8,
    TirePressure = 9,
    DoorOpenStatus = 10,
    IgnitionStatus = 11,
    Temperature = 12
}