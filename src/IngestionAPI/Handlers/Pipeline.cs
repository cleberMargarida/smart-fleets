using IngestionAPI.Handlers.Abstractions;

namespace IngestionAPI.Handlers
{
    /// <summary>
    /// Represents a pipeline of handlers that process a signal in sequence.
    /// </summary>
    public class Pipeline : IPipeline
    {
        private readonly IHandler[] _handlers;
        private readonly ILogger<Pipeline> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="Pipeline"/> class.
        /// </summary>
        /// <param name="handlers">The collection of handlers to be executed in the pipeline.</param>
        /// <param name="logger">The logger for logging errors during handler execution.</param>
        /// <exception cref="PipelineConfiguratorException">Thrown if a handler implements more than one functionality.</exception>
        public Pipeline(IEnumerable<IHandler> handlers, ILogger<Pipeline> logger)
        {
            _handlers = handlers.ToArray();
            foreach (var handler in _handlers)
            {
                if (handler is IValidatorHandler and IBlockingHandler or
                               IValidatorHandler and IAsyncHandler or
                               IBlockingHandler and IAsyncHandler)
                {
                    throw new PipelineConfiguratorException($"The type {handler.GetType()} are implementing more than one functionality. Handlers can only implement one of IValidatorHandler, IBlockingHandler, or IAsyncHandler.");
                }
            }
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task RunAsync(Signal signal)
        {
            foreach (var handler in _handlers)
            {
                try
                {
                    if (handler is IValidatorHandler validator)
                    {
                        if (!validator.Handle(signal))
                        {
                            break;
                        }
                    }

                    if (handler is IBlockingHandler blocking)
                    {
                        blocking.Handle(signal);
                        continue;
                    }

                    if (handler is IAsyncHandler asyncHandler)
                    {
                        await asyncHandler.HandleAsync(signal);
                        continue;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError("An error occurred during the execution of the pipeline. The handler {handler} failed. {ex}", handler, ex);
                    throw;
                }
            }
        }
    }
}
