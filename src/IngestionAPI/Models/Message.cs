using MessagePack;

namespace IngestionAPI.Models
{
    /// <summary>
    /// Represents a message containing a collection of signals.
    /// </summary>
    [MessagePackObject(keyAsPropertyName: true)]
    public class Message
    {
        /// <summary>
        /// Gets or sets the unique identifier of the message.
        /// </summary>
        public required string Id { get; set; }

        /// <summary>
        /// Gets or sets the list of signals contained in the message.
        /// </summary>
        public List<Signal> Signals { get; set; } = new List<Signal>();
    }
}
