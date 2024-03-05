using ServiceModels.Abstractions;

namespace Ingestion.Api.Handlers.Abstractions
{
    /// <summary>
    /// Defines a Synchronously handler for processing signals within a pipeline.
    /// </summary>
    public interface IBlockingHandler : IHandler
    {
        /// <summary>
        /// Synchronously Handles a signal.
        /// </summary>
        /// <param name="signal">The signal to be handled.</param>
        /// <remarks>
        /// Used for shortly operations.
        /// </remarks>
        void Handle(BaseSignal signal);
    }
}

