using ServiceModels;
using ServiceModels.Abstractions;

namespace IngestionAPI.GrainInterfaces;

/// <summary>
/// Represents a grain interface for managing the current state information of a vehicle.
/// </summary>
public interface IVehicleStateGrain : IGrainWithVehicleIdKey
{
    /// <summary>
    /// Adds a new signal to the state of the vehicle or updates an existing signal.
    /// </summary>
    /// <param name="signal">The signal to add or update.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task AddOrUpdateAsync(BaseSignal signal);

    /// <summary>
    /// Retrieves the current state of the vehicle.
    /// </summary>
    /// <returns>A value task containing the current state of the vehicle.</returns>
    ValueTask<VehicleState> GetStateAsync();
}
