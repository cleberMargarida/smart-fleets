using MessagePack;

namespace ServiceModels
{
    [MessagePackObject(keyAsPropertyName: true)]
    [GenerateSerializer]
    public class VehicleHistoricalState : VehicleState
    {
    }
}
