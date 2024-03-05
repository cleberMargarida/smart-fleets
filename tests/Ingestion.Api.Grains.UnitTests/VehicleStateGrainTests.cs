using Orleans.Core;
using ServiceModels;
using SmartFleets.RabbitMQ.Base;

namespace Ingestion.Api.Grains.UnitTests
{
    public class VehicleStateGrainTests
    {
        private readonly VehicleStateGrain _grain;
        private readonly Mock<IBus> _bus;
        private readonly Mock<IStorage<VehicleState>> _storage;
        private readonly Mock<IVehicleHistoricalStateGrain> _historicalStateGrain;

        public VehicleStateGrainTests(VehicleStateGrain grain, Mock<IBus> bus, Mock<IStorage<VehicleState>> storage, Mock<IVehicleHistoricalStateGrain> historicalStateGrain )
        {
            _grain = grain;
            _bus = bus;
            _storage = storage;
            _historicalStateGrain = historicalStateGrain;
        }

        [Fact]
        public async Task AddOrUpdateAsync_WhenSignalIsNewer_ShouldUpdateStateAndPublish()
        {
            // Arrange
            var older = Activator.CreateInstance<Speed>();
            older.Id = Guid.NewGuid().ToString();
            older.Value = 1;
            older.DateTimeUtc = DateTime.UtcNow - TimeSpan.FromMinutes(2);

            var newer = Activator.CreateInstance<Speed>();
            newer.Id = Guid.NewGuid().ToString();
            newer.Value = 1;
            newer.DateTimeUtc = DateTime.UtcNow;

            _storage.SetupGet(proxy => proxy.State).Returns(new VehicleState { Speed = older });

            // Act
            await _grain.AddOrUpdateAsync(newer);

            // Assert
            _bus.Verify(proxy => proxy.PublishAsync(It.IsAny<VehicleState>()), Times.Once);

            var currentState = await _grain.GetStateAsync();
            var result = currentState[newer.Type];

            result.Should().BeEquivalentTo(newer);
            result.Should().NotBeEquivalentTo(older);
        }

        [Fact]
        public async Task AddOrUpdateAsync_WhenSignalIsOlder_ShouldNotUpdateStateOrPublish_And_ShouldAddToHistoricalStateGrain()
        {
            // Arrange
            var older = Activator.CreateInstance<Speed>();
            older.Id = Guid.NewGuid().ToString();
            older.Value = 1;
            older.DateTimeUtc = DateTime.UtcNow - TimeSpan.FromMinutes(2);

            var newer = Activator.CreateInstance<Speed>();
            newer.Id = Guid.NewGuid().ToString();
            newer.Value = 1;
            newer.DateTimeUtc = DateTime.UtcNow;

            _storage.SetupGet(proxy => proxy.State).Returns(new VehicleState { Speed = newer });

            // Act
            await _grain.AddOrUpdateAsync(older);

            // Assert
            _bus.Verify(proxy => proxy.PublishAsync(It.IsAny<VehicleState>()), Times.Never);
            _historicalStateGrain.Verify(proxy => proxy.AddAsync(It.IsAny<Speed>()));

            var currentState = await _grain.GetStateAsync();
            var result = currentState[newer.Type];

            result.Should().BeEquivalentTo(newer);
            result.Should().NotBeEquivalentTo(older);
        }
    }
}

