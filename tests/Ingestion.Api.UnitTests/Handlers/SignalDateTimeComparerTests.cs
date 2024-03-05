using ServiceModels;

namespace Ingestion.Api.Handlers.Tests
{
    public class SignalDateTimeComparerTests
    {
        [Fact]
        public void Compare_EqualDateTimes_ReturnsZero()
        {
            // Arrange
            var comparer = new SignalDateTimeComparer();
            var dateTime = DateTime.Now;
            var signal1 = Activator.CreateInstance<Speed>();
            signal1.DateTimeUtc = dateTime;
            var signal2 = Activator.CreateInstance<Speed>();
            signal2.DateTimeUtc = dateTime;

            // Act
            var result = comparer.Compare(signal1, signal2);

            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void Compare_FirstDateTimeEarlier_ReturnsNegative()
        {
            // Arrange
            var comparer = new SignalDateTimeComparer();
            var signal1 = Activator.CreateInstance<Speed>();
            signal1.DateTimeUtc = DateTime.Now;
            var signal2 = Activator.CreateInstance<Speed>();
            signal2.DateTimeUtc = DateTime.Now.AddSeconds(1);

            // Act
            var result = comparer.Compare(signal1, signal2);

            // Assert
            Assert.True(result < 0);
        }

        [Fact]
        public void Compare_SecondDateTimeEarlier_ReturnsPositive()
        {
            // Arrange
            var comparer = new SignalDateTimeComparer();
            var signal1 = Activator.CreateInstance<Speed>();
            signal1.DateTimeUtc = DateTime.Now.AddSeconds(1);
            var signal2 = Activator.CreateInstance<Speed>();
            signal2.DateTimeUtc = DateTime.Now;

            // Act
            var result = comparer.Compare(signal1, signal2);

            // Assert
            Assert.True(result > 0);
        }

        [Fact]
        public void Compare_FirstSignalNull_ReturnsNegative()
        {
            // Arrange
            var comparer = new SignalDateTimeComparer();
            Speed signal1 = null;
            var signal2 = Activator.CreateInstance<Speed>();
            signal2.DateTimeUtc = DateTime.Now;

            // Act
            var result = comparer.Compare(signal1, signal2);

            // Assert
            Assert.True(result < 0);
        }

        [Fact]
        public void Compare_SecondSignalNull_ReturnsPositive()
        {
            // Arrange
            var comparer = new SignalDateTimeComparer();
            var signal1 = Activator.CreateInstance<Speed>();
            signal1.DateTimeUtc = DateTime.Now;
            Speed signal2 = null;

            // Act
            var result = comparer.Compare(signal1, signal2);

            // Assert
            Assert.True(result > 0);
        }

        [Fact]
        public void Compare_BothSignalsNull_ReturnsZero()
        {
            // Arrange
            var comparer = new SignalDateTimeComparer();
            Speed signal1 = null;
            Speed signal2 = null;

            // Act
            var result = comparer.Compare(signal1, signal2);

            // Assert
            Assert.Equal(0, result);
        }
    }
}

