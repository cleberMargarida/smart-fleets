using ServiceModels;
using ServiceModels.Abstractions;
using SmartFleets.RabbitMQ.Base;

namespace IngestionAPI.GrainInterfaces;

public class VehicleStateGrain : Grain<VehicleState>, IVehicleStateGrain
{
    private readonly IBus _bus;

    public VehicleStateGrain(IBus bus)
    {
        _bus = bus;
    }

    public async Task AddOrUpdateAsync(BaseSignal signal)
    {
        if (signal.IsNewerThan(State[signal.Type]))
        {
            State[signal.Type] = signal;

            await _bus.PublishAsync(State);
            await WriteStateAsync();
        }

        var vehicleHistoricalStateGrain = GrainFactory.GetGrain<IVehicleHistoricalStateGrain>(
                signal.VehicleId,
                signal.DateTimeUtc);

        vehicleHistoricalStateGrain.AddAsync(signal).Ignore();
    }

    public ValueTask<VehicleState> GetStateAsync()
    {
        return ValueTask.FromResult(State);
    }
}