using Orleans.Concurrency;
using ServiceModels;
using ServiceModels.Abstractions;

namespace IngestionAPI.GrainInterfaces;

public interface IVehicleHistoricalStateGrain : IGrainWithVehicleIdAndDateTimeKey
{
    Task AddAsync(BaseSignal signal);
    ValueTask<VehicleHistoricalState> GetStateAsync();
}