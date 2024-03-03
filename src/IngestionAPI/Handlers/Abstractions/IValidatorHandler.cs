using ServiceModels.Abstractions;

namespace IngestionAPI.Handlers.Abstractions
{
    /// <summary>
    /// Defines a validator handler for processing signals within a pipeline.
    /// This handler has the ability to determine whether the processing should
    /// continue to the next handler in the chain of responsibility.
    /// </summary>
    public interface IValidatorHandler : IHandler
    {
        /// <summary>
        /// Handles a signal and determines whether it is valid.
        /// If the signal is valid, processing can continue to the next handler
        /// in the chain of responsibility. If not, processing stops.
        /// </summary>
        /// <param name="signal">The signal to be validated.</param>
        /// <returns>A boolean indicating whether the signal is valid and processing should continue.</returns>
        bool Handle(BaseSignal signal);
    }
}
