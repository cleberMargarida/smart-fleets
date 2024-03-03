using IngestionAPI.GrainInterfaces;
using IngestionAPI.Handlers.Abstractions;
using ServiceModels.Abstractions;

public class VehicleStateHandler : IAsyncHandler
{
    private readonly IGrainFactory _factory;

    public VehicleStateHandler(IGrainFactory factory)
    {
        _factory = factory;
    }

    public Task HandleAsync(BaseSignal signal)
    {
        var state = _factory.GetGrain<IVehicleStateGrain>(signal.VehicleId);
        return state.AddOrUpdateAsync(signal);
    }
}