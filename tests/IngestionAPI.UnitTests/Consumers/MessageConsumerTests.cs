using IngestionAPI.Consumers;
using IngestionAPI.Handlers.Abstractions;
using IngestionAPI.Models;
using Moq;

namespace IngestionAPI.UnitTests.Consumers
{
    public class MessageConsumerTests
    {
        private readonly MessageConsumer _consumer;
        private readonly Mock<IPipeline> _pipeline;

        public MessageConsumerTests(MessageConsumer consumer, Mock<IPipeline> pipeline)
        {
            _consumer = consumer;
            _pipeline = pipeline;
        }

        [Fact]
        public async Task ConsumeAsync_MessageWithSignals_ExecutesPipelineForEachSignal()
        {
            // Arrange
            var message = Activator.CreateInstance<Message>();
            message.Signals = [Activator.CreateInstance<Signal>(), Activator.CreateInstance<Signal>()];

            // Act
            await _consumer.ConsumeAsync(message);

            // Assert
            _pipeline.Verify(p => p.RunAsync(It.IsAny<Signal>()), Times.Exactly(message.Signals.Count));
        }

        [Fact]
        public async Task ConsumeAsync_EmptyMessage_DoesNotExecutePipeline()
        {
            // Arrange
            var message = Activator.CreateInstance<Message>();
            message.Signals = [];

            // Act
            await _consumer.ConsumeAsync(message);

            // Assert
            _pipeline.Verify(p => p.RunAsync(It.IsAny<Signal>()), Times.Never);
        }
    }
}
