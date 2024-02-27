using Azure.Messaging.EventHubs.Processor;

namespace IngestionAPI.Handlers.MessageErrorHandling
{
    public interface IMessageErrorHandler
    {
    }

    public interface IMessageErrorHandler<T> : IMessageErrorHandler
    {
        Task HandleError(ProcessErrorEventArgs error);
    }
}