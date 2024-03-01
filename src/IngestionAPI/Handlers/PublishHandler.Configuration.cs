using System.Text.Json.Serialization;

namespace IngestionAPI.Handlers
{
    /// <summary>
    /// Represents the configuration options for the PublishHandler.
    /// </summary>
    public class PublishHandlerConfiguration
    {
        /// <summary>
        /// Gets a value indicating whether the batch size should be used.
        /// </summary>
        [JsonIgnore]
        public bool UseBatchSize => BatchSize > 0;

        /// <summary>
        /// Gets a value indicating whether the timeout should be used.
        /// </summary>
        [JsonIgnore]
        public bool UseTimeout => Timeout != TimeSpan.Zero;

        /// <summary>
        /// Gets a value indicating whether the scan period should be used.
        /// </summary>
        [JsonIgnore]
        public bool UseScanPeriod => ScanPeriod != TimeSpan.Zero;

        /// <summary>
        /// Gets or sets the batch size for publishing signals.
        /// </summary>
        public int BatchSize { get; set; }

        /// <summary>
        /// Gets or sets the timeout for publishing signals.
        /// </summary>
        public TimeSpan Timeout { get; set; }

        /// <summary>
        /// Gets or sets the scan period for checking and publishing signals.
        /// </summary>
        public TimeSpan ScanPeriod { get; set; }
    }
}
