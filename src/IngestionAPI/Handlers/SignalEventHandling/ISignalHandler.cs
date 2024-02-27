namespace IngestionAPI.Handlers.SignalEventHandling
{
    public interface ISignalHandler
    {
    }

    public interface ISignalHandler<T> : ISignalHandler
    {
        Task HandleAsync(T signal);
    }
}