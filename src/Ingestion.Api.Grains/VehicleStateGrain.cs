using ServiceModels;
using ServiceModels.Abstractions;
using SmartFleets.RabbitMQ.Base;

namespace Ingestion.Api.GrainInterfaces;

public class VehicleStateGrain : Grain<VehicleState>, IVehicleStateGrain
{
    private readonly IBus _bus;

    public VehicleStateGrain(IBus bus)
    {
        _bus = bus;
    }

    /// <inheritdoc/>
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

    /// <inheritdoc/>
    public ValueTask<VehicleState> GetStateAsync()
    {
        return ValueTask.FromResult(State);
    }
}
