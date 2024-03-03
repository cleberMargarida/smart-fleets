using Orleans.Core;
using ServiceModels;
using ServiceModels.Abstractions;
using SmartFleets.RabbitMQ.Base;

namespace IngestionAPI.GrainInterfaces
{
    public class VehicleHistoricalStateGrain : Grain<VehicleHistoricalState>, IVehicleHistoricalStateGrain
    {
        private readonly IBus _bus;

        public VehicleHistoricalStateGrain(IBus bus)
        {
            _bus = bus;
        }

        public async Task AddAsync(BaseSignal signal)
        {
            State[signal.Type] = signal;

            await WriteStateAsync();
            await _bus.PublishAsync(State);
        }

        public ValueTask<VehicleHistoricalState> GetStateAsync()
        {
            return ValueTask.FromResult(State);
        }
    }
}