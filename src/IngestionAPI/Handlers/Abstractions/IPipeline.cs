namespace IngestionAPI.Handlers.Abstractions
{
    /// <summary>
    /// Asynchronously executes the pipeline on a given signal.
    /// </summary>
    /// <param name="signal">The signal to be processed by the pipeline.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public interface IPipeline
    {
        Task RunAsync(Signal signal);
    }
}
