using Ingestion.Api.Handlers;
using Ingestion.Api.Models;
using ServiceModels;

namespace Ingestion.Api.Tests.Handlers
{
    public class EnrichHandlerTests
    {
        private readonly EnrichHandler _handler;

        public EnrichHandlerTests(EnrichHandler handler)
        {
            _handler = handler;
        }

        [Fact]
        public void Handle_SignalProvided_EnrichesSignalWithIngestedDateTimeUtc()
        {
            // Arrange
            var signal = Activator.CreateInstance<Speed>();

            // Act
            _handler.Handle(signal);

            // Assert
            Assert.True((DateTime.UtcNow - signal.IngestedDateTimeUtc).TotalSeconds < 1);
        }
    }
}

