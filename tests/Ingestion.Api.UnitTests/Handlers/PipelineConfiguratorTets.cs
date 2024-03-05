using Ingestion.Api.Handlers;
using Ingestion.Api.Handlers.Abstractions;

namespace Ingestion.Api.UnitTests.Handlers
{
    public class PipelineConfiguratorTests
    {
        [Fact]
        public void Add_ValidHandler_AddsHandlerToPipeline()
        {
            // Arrange
            var configurator = new PipelineConfigurator();

            // Act
            configurator.Add<MockHandler>();

            // Assert
            var pipeline = (HashSet<Type>)configurator;
            Assert.Contains(typeof(MockHandler), pipeline);
        }

        [Fact]
        public void Add_DuplicateHandler_ThrowsPipelineConfiguratorException()
        {
            // Arrange
            var configurator = new PipelineConfigurator();
            configurator.Add<MockHandler>();

            // Act & Assert
            Assert.Throws<PipelineConfiguratorException>(() => configurator.Add<MockHandler>());
        }

        private class MockHandler : IHandler
        {
            // Implementation of the IHandler interface
        }
    }
}

