using IngestionAPI.Handlers.Abstractions;
using System;

namespace IngestionAPI.Handlers
{
    /// <summary>
    /// A handler that enriches a signal with additional informations.
    /// </summary>
    public class EnrichHandler : IBlockingHandler
    {
        /// <summary>
        /// Enriches the given signal with additional informations.
        /// </summary>
        /// <param name="signal">The signal to be enriched.</param>
        public void Handle(Signal signal)
        {
            signal.IngestedDateTimeUtc = DateTime.UtcNow;
        }
    }
}
