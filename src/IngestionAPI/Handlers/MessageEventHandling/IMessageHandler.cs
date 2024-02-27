namespace IngestionAPI.Handlers.MessageEventHandling
{
    public interface IMessageHandler
    {
    }

    public interface IMessageHandler<T> : IMessageHandler
    {
        Task HandleAsync(T message);
    }
}