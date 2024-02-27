namespace IngestionAPI.Handlers.MessageEventValidation
{
    public interface IMessageValidator
    {
    }

    public interface IMessageValidator<T> : IMessageValidator
    {
        Task<bool> IsValid(T message);
    }
}