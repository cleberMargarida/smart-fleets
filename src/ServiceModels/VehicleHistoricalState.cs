using MessagePack;

namespace ServiceModels
{
    /// <summary>
    /// Represents the historical state of a vehicle, extending the <see cref="VehicleState"/> class.
    /// </summary>
    [MessagePackObject(keyAsPropertyName: true)]
    [GenerateSerializer]
    public class VehicleHistoricalState : VehicleState
    {
    }
}
