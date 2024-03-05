using ServiceModels;
using ServiceModels.Abstractions;

namespace Ingestion.GrainInterfaces;

/// <summary>
/// Represents a grain interface for managing historical state information of a vehicle.
/// </summary>
public interface IVehicleHistoricalStateGrain : IGrainWithVehicleIdAndDateTimeKey
{
    /// <summary>
    /// Adds a new signal to the historical state of the vehicle.
    /// </summary>
    /// <param name="signal">The signal to add.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task AddAsync(BaseSignal signal);

    /// <summary>
    /// Retrieves the current historical state of the vehicle.
    /// </summary>
    /// <returns>A value task containing the historical state of the vehicle.</returns>
    ValueTask<VehicleHistoricalState> GetStateAsync();
}


