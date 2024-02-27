namespace IngestionAPI.Handlers.SignalEventValidation
{
    public interface ISignalValidator
    {
    }

    public interface ISignalValidator<T>
    {
        Task<bool> IsValid(T signal);
    }
}