using Ingestion.GrainInterfaces;
using ServiceModels;
using ServiceModels.Abstractions;
using SmartFleets.RabbitMQ.Base;

namespace Ingestion.Grains;

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
        if (signal.IsOlderThan(State[signal.Type]))
        {
            return;
        }

        State[signal.Type] = signal;

        await _bus.PublishAsync(State);
        await WriteStateAsync();
    }

    /// <inheritdoc/>
    public ValueTask<VehicleState> GetStateAsync()
    {
        return ValueTask.FromResult(State);
    }
}

