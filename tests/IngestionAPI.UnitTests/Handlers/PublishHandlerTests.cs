using IngestionAPI.Models;
using Microsoft.Extensions.Options;
using Moq;
using ServiceModels;
using ServiceModels.Binding;
using SmartFleets.RabbitMQ.Base;

namespace IngestionAPI.Handlers.Tests
{
    public class PublishHandlerTests
    {
        private readonly PublishHandler _handler;
        private readonly Mock<IBus> _bus;
        private readonly Mock<IOptions<PublishHandlerConfiguration>> _publishHandlerConfiguration;

        public PublishHandlerTests(
            PublishHandler handler,
            Mock<IBus> bus,
            Mock<IOptions<PublishHandlerConfiguration>> publishHandlerConfiguration)
        {
            _handler = handler;
            _bus = bus;
            _publishHandlerConfiguration = publishHandlerConfiguration;
        }

        [Fact]
        public async Task HandleAsync_BatchSizeReached_PublishesSignals()
        {
            // Arrange
            var configuration = new PublishHandlerConfiguration { BatchSize = 2 };
            _publishHandlerConfiguration.Setup(c => c.Value).Returns(configuration);

            var signal1 = Activator.CreateInstance<Signal>();
            signal1.Type = (uint)SignalType.Speed;
            signal1.Value = 1;
            signal1.DateTimeUtc = DateTime.UtcNow;

            var signal2 = Activator.CreateInstance<Signal>();
            signal2.Type = (uint)SignalType.Speed;
            signal2.Value = 1;
            signal2.DateTimeUtc = DateTime.UtcNow;

            // Act
            await _handler.HandleAsync(signal1);
            await _handler.HandleAsync(signal2); // Add another signal to reach batch size

            // Assert
            _bus.Verify(b => b.PublishBatchAsync(It.IsAny<IEnumerable<Speed>>()), Times.Exactly(1));
        }

        [Fact]
        public async Task HandleAsync_TimeoutReached_PublishesSignals()
        {
            // Arrange
            var configuration = new PublishHandlerConfiguration { Timeout = TimeSpan.FromMilliseconds(50) };
            _publishHandlerConfiguration.Setup(c => c.Value).Returns(configuration);

            var signal1 = Activator.CreateInstance<Signal>();
            signal1.Type = (uint)SignalType.Speed;
            signal1.Value = 1;
            signal1.DateTimeUtc = DateTime.UtcNow;

            var signal2 = Activator.CreateInstance<Signal>();
            signal2.Type = (uint)SignalType.Speed;
            signal2.Value = 1;
            signal2.DateTimeUtc = DateTime.UtcNow;

            // Act
            await _handler.HandleAsync(signal1);
            await Task.Delay(100); // Wait for timeout to be reached
            await _handler.HandleAsync(signal2); // Trigger another handle to check if signals are published

            // Assert
            _bus.Verify(b => b.PublishBatchAsync(It.IsAny<IEnumerable<Speed>>()), Times.Exactly(1));
        }

        [Fact]
        public async Task HandleAsync_SignalTypeDifferent_AddsToCorrectSet()
        {
            // Arrange
            var signalType1 = Activator.CreateInstance<Signal>();
            signalType1.Type = (uint)SignalType.Speed;
            signalType1.Value = 1;

            var signalType2 = Activator.CreateInstance<Signal>();
            signalType2.Type = (uint)SignalType.FuelLevel;
            signalType2.Value = 1;

            // Act
            await _handler.HandleAsync(signalType1);
            await _handler.HandleAsync(signalType2);

            // Assert
            _bus.Verify(b => b.PublishBatchAsync(It.IsAny<IEnumerable<Speed>>()), Times.Exactly(1));
            _bus.Verify(b => b.PublishBatchAsync(It.IsAny<IEnumerable<FuelLevel>>()), Times.Exactly(1));
        }

        [Fact]
        public async Task PeriodicScan_ScanPeriodElapsed_PublishesAllSignals()
        {
            // Arrange
            var configuration = new PublishHandlerConfiguration
            {
                BatchSize = 2,
                Timeout = TimeSpan.FromMilliseconds(50),
                ScanPeriod = TimeSpan.FromMilliseconds(100)
            };
            _publishHandlerConfiguration.Setup(c => c.Value).Returns(configuration);

            var signal = Activator.CreateInstance<Signal>();
            signal.Type = (uint)SignalType.Speed;
            signal.Value = 1;

            // Act
            await _handler.HandleAsync(signal);
            await Task.Delay(150); // Wait for scan period to elapse

            // Assert
            _bus.Verify(b => b.PublishBatchAsync(It.IsAny<IEnumerable<Speed>>()), Times.Once);
        }

        [Fact]
        public async Task PeriodicScan_ScanPeriodNotElapsed_PublishesAllSignals()
        {
            // Arrange
            var configuration = new PublishHandlerConfiguration
            {
                BatchSize = 2,
                Timeout = TimeSpan.FromMilliseconds(100),
                ScanPeriod = TimeSpan.FromMilliseconds(100)
            };
            _publishHandlerConfiguration.Setup(c => c.Value).Returns(configuration);

            var signal = Activator.CreateInstance<Signal>();
            signal.Type = (uint)SignalType.Speed;
            signal.Value = 1;

            // Act
            await _handler.HandleAsync(signal);

            // Assert
            _bus.Verify(b => b.PublishBatchAsync(It.IsAny<IEnumerable<Speed>>()), Times.Never);
        }

        [Fact]
        public async Task HandleAsync_NoBatchSizeOrTimeout_PublishImmediately()
        {
            // Arrange
            var signal = Activator.CreateInstance<Signal>();
            signal.Type = (uint)SignalType.Speed;
            signal.Value = 1;

            // Act
            await _handler.HandleAsync(signal);

            // Assert
            _bus.Verify(b => b.PublishBatchAsync(It.IsAny<IEnumerable<Speed>>()), Times.Once);
        }

        [Fact]
        public async Task HandleAsync_SignalAddedAfterDispose_DoesNotThrowException()
        {
            // Arrange
            _handler.Dispose();

            var signal = Activator.CreateInstance<Signal>();
            signal.Type = (uint)SignalType.Speed;
            signal.Value = 1;

            // Act & Assert
            var exception = await Record.ExceptionAsync(() => _handler.HandleAsync(signal));
            Assert.Null(exception);
        }

        [Fact]
        public async Task HandleAsync_SignalAdded_MappedCorrectly()
        {
            // Arrange
            var expected = Activator.CreateInstance<Signal>();
            expected.Type = (uint)SignalType.Speed;
            expected.Value = 1;
            expected.DateTimeUtc = DateTime.UtcNow;
            expected.Id = Guid.NewGuid().ToString();
            expected.VehicleId = Guid.NewGuid().ToString();

            Speed actual = null;
            _bus.Setup(b => b.PublishBatchAsync(It.IsAny<IEnumerable<Speed>>()))
                .Callback<IEnumerable<Speed>>(s => actual = s.First());

            // Act
            await _handler.HandleAsync(expected);

            // Assert
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal((SignalType)expected.Type, actual.Type);
            Assert.Equal(expected.Value, actual.Value);
            Assert.Equal(expected.DateTimeUtc, actual.DateTimeUtc);
            Assert.Equal(expected.VehicleId, actual.VehicleId);
        }

        [Fact]
        public async Task HandleAsync_MultipleSignalsAdded_OrdersCorrectly()
        {
            // Arrange
            var configuration = new PublishHandlerConfiguration { BatchSize = 2 };
            _publishHandlerConfiguration.Setup(c => c.Value).Returns(configuration);

            var signal1 = Activator.CreateInstance<Signal>();
            signal1.Type = (uint)SignalType.Speed;
            signal1.Value = 1;
            signal1.DateTimeUtc = DateTime.UtcNow;

            var signal2 = Activator.CreateInstance<Signal>();
            signal2.Type = (uint)SignalType.Speed;
            signal2.Value = 1;
            signal2.DateTimeUtc = DateTime.UtcNow - TimeSpan.FromMinutes(2);

            List<Speed> captured = [];
            _bus.Setup(b => b.PublishBatchAsync(It.IsAny<IEnumerable<Speed>>()))
                .Callback<IEnumerable<Speed>>(captured.AddRange);

            // Act
            await _handler.HandleAsync(signal1);
            await _handler.HandleAsync(signal2); // 2 minutes older signal published after

            // Assert
            Assert.True(captured.Count == 2);
            Assert.True(captured.First().DateTimeUtc < captured.Last().DateTimeUtc);
        }

        [Fact]
        public async Task HandleAsync_SignalsWithIdenticalTimestamps_ShouldSingle()
        {
            // Arrange
            var configuration = new PublishHandlerConfiguration { Timeout = TimeSpan.FromMilliseconds(50) };
            _publishHandlerConfiguration.Setup(c => c.Value).Returns(configuration);

            var signal1 = Activator.CreateInstance<Signal>();
            signal1.Type = (uint)SignalType.Speed;
            signal1.Value = 1;
            signal1.DateTimeUtc = DateTime.UtcNow;

            var signal2 = Activator.CreateInstance<Signal>();
            signal2.Type = (uint)SignalType.Speed;
            signal2.Value = 1;
            signal2.DateTimeUtc = signal1.DateTimeUtc;

            List<Speed> captured = [];
            _bus.Setup(b => b.PublishBatchAsync(It.IsAny<IEnumerable<Speed>>()))
                .Callback<IEnumerable<Speed>>(captured.AddRange);

            // Act
            await _handler.HandleAsync(signal1);
            await Task.Delay(50);
            await _handler.HandleAsync(signal2);

            // Assert
            Assert.Single(captured);
        }

        [Fact]
        public async Task HandleAsync_SignalTypeNotSupported_DoesNotAddToSet()
        {
            // Arrange
            var unsupportedSignal = Activator.CreateInstance<Signal>();
            unsupportedSignal.Type = 999;
            unsupportedSignal.Value = 0;

            // Act
            Exception error = await Record.ExceptionAsync(() => _handler.HandleAsync(unsupportedSignal));

            // Assert
            Assert.NotNull(error);
        }
    }
}
