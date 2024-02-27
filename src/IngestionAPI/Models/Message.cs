using MessagePack;

namespace IngestionAPI.Models
{
    [MessagePackObject(keyAsPropertyName: true)]
    public class Message
    {
        public required string Id { get; set; }
        public List<Signal> Signals { get; set; } = [];
    }
}