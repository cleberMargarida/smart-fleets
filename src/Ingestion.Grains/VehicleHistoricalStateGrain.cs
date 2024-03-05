using Ingestion.GrainInterfaces;
using Orleans.Core;
using ServiceModels;
using ServiceModels.Abstractions;
using SmartFleets.RabbitMQ.Base;

namespace Ingestion.Grains
{
    public class VehicleHistoricalStateGrain : Grain<VehicleHistoricalState>, IVehicleHistoricalStateGrain
    {
        private readonly IBus _bus;

        public VehicleHistoricalStateGrain(IBus bus)
        {
            _bus = bus;
        }

        /// <inheritdoc/>
        public async Task AddAsync(BaseSignal signal)
        {
            State[signal.Type] = signal;

            await WriteStateAsync();
            await _bus.PublishAsync(State);
        }

        /// <inheritdoc/>
        public ValueTask<VehicleHistoricalState> GetStateAsync()
        {
            return ValueTask.FromResult(State);
        }
    }
}

