using ServiceModels.Abstractions;

namespace Ingestion.Api.Handlers.Abstractions
{
    /// <summary>
    /// Defines an asynchronous handler for processing signals within a pipeline.
    /// </summary>
    public interface IAsyncHandler : IHandler
    {
        /// <summary>
        /// Asynchronously handles a signal.
        /// </summary>
        /// <param name="signal">The signal to be handled asynchronously.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <remarks>
        /// It doesn't blocks the current thread, used for long operations.
        /// </remarks>
        Task HandleAsync(BaseSignal signal);
    }
}

