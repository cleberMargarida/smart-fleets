using IngestionAPI.Handlers.Abstractions;
using SmartFleets.RabbitMQ.Messaging;

namespace IngestionAPI.Consumers
{
    /// <summary>
    /// Consumes messages and processes them using a specified pipeline.
    /// </summary>
    public class MessageConsumer : IConsumer<Message>
    {
        private readonly IPipeline _pipeline;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageConsumer"/> class.
        /// </summary>
        /// <param name="pipeline">The pipeline used to execute processing on received messages.</param>
        public MessageConsumer(IPipeline pipeline)
        {
            _pipeline = pipeline;
        }

        /// <inheritdoc/>
        public async Task ConsumeAsync(Message message) 
        {
            foreach (var signal in message.Signals)
            {
                await _pipeline.RunAsync(signal);
            }
        }
    }
}
