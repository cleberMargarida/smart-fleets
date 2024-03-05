using ServiceModels;
using SmartFleets.RabbitMQ.Base;

namespace Ingestion.Api.Grains.UnitTests
{
    public class VehicleHistoricalStateGrainTests
    {
        private readonly VehicleHistoricalStateGrain _grain;
        private readonly Mock<IBus> _bus;

        public VehicleHistoricalStateGrainTests(
            VehicleHistoricalStateGrain grain, Mock<IBus> bus)
        {
            _grain = grain;
            _bus = bus;
        }

        [Fact]
        public async Task AddAsync_WhenSignalIsProvided_ShouldUpdateStateAndPublish()
        {
            // Arrange
            var signal = Activator.CreateInstance<Speed>();
            signal.Id = Guid.NewGuid().ToString();
            signal.Value = 1;
            signal.DateTimeUtc = DateTime.UtcNow;

            // Act
            await _grain.AddAsync(signal);

            // Assert
            var currentState = await _grain.GetStateAsync();
            var result = currentState[signal.Type];
            signal.Should().BeEquivalentTo(result);
            _bus.Verify(proxy => proxy.PublishAsync(It.IsAny<VehicleState>()), Times.Once);
        }
    }
}

