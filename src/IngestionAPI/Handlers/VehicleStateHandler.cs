using IngestionAPI.GrainInterfaces;
using IngestionAPI.Handlers.Abstractions;
using ServiceModels.Abstractions;

namespace IngestionAPI.Handlers
{

    /// <summary>
    /// Handles the processing of vehicle state signals.
    /// </summary>
    public class VehicleStateHandler : IAsyncHandler
    {
        private readonly IGrainFactory _factory;

        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleStateHandler"/> class.
        /// </summary>
        /// <param name="factory">The grain factory used to get grain references.</param>
        public VehicleStateHandler(IGrainFactory factory)
        {
            _factory = factory;
        }

        /// <summary>
        /// Handles the processing of a vehicle state signal asynchronously.
        /// </summary>
        /// <param name="signal">The vehicle state signal to be processed.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public Task HandleAsync(BaseSignal signal)
        {
            var state = _factory.GetGrain<IVehicleStateGrain>(signal.VehicleId);
            return state.AddOrUpdateAsync(signal);
        }
    }
}