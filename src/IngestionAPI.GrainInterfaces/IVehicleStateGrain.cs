using ServiceModels;
using ServiceModels.Abstractions;

namespace IngestionAPI.GrainInterfaces;

public interface IVehicleStateGrain : IGrainWithVehicleIdKey
{
    Task AddOrUpdateAsync(BaseSignal signal);
    ValueTask<VehicleState> GetStateAsync();
}