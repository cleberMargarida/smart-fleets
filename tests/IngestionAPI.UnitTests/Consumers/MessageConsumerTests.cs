using IngestionAPI.Consumers;
using IngestionAPI.Models;
using Moq;
using SmartFleet.RabbitMQ.Base;

namespace IngestionAPI.UnitTests.Consumers
{
    public class MessageConsumerTests
    {
        private readonly MessageConsumer _consumer;
        private readonly Mock<IBus> _bus;

        public MessageConsumerTests(MessageConsumer consumer, Mock<IBus> bus)
        {
            _consumer = consumer;
            _bus = bus;
        }

        [Fact]
        public async Task ConsumeAsync_MessageContaningSpeed_ShouldPublish()
        {
            // Arrange
            var signal = new Signal
            {
                Id = Guid.NewGuid().ToString(),
                TenantId = 2,
                Value = 1,
                VehicleId = Guid.NewGuid().ToString(),
                SignalType = 1,
                DateTime = DateTime.Now,
            };
            var message = new Message
            {
                Id = Guid.NewGuid().ToString(),
                Signals = { signal }
            };

            // Act
            await _consumer.ConsumeAsync(message);

            // Assert
            _bus.Verify(b => b.PublishAsync(It.IsAny<object>()), Times.Once);
        }
    }
}
