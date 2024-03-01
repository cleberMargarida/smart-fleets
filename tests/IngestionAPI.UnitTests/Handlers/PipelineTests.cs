using IngestionAPI.Handlers;
using IngestionAPI.Handlers.Abstractions;
using IngestionAPI.Models;
using Microsoft.Extensions.Logging;
using Moq;

namespace IngestionAPI.Tests.Handlers
{
    public class PipelineTests
    {
        private readonly Pipeline _pipeline;
        private readonly Mock<IValidatorHandler> _validatorHandler;
        private readonly Mock<IBlockingHandler> _syncHandler;
        private readonly Mock<IAsyncHandler> _asyncHandler;

        public PipelineTests(Pipeline pipeline,
            Mock<IValidatorHandler> validatorHandler,
            Mock<IBlockingHandler> syncHandler,
            Mock<IAsyncHandler> asyncHandler)
        {
            _pipeline = pipeline;
            _validatorHandler = validatorHandler;
            _syncHandler = syncHandler;
            _asyncHandler = asyncHandler;
        }

        [Fact]
        public async Task RunAsync_ValidSignal_ExecutesAllHandlers()
        {
            var signal = Activator.CreateInstance<Signal>();
            _validatorHandler.Setup(h => h.Handle(It.IsAny<Signal>())).Returns(true);

            // Act
            await _pipeline.RunAsync(signal);

            // Assert
            _validatorHandler.Verify(h => h.Handle(It.IsAny<Signal>()), Times.Once);
            _syncHandler.Verify(h => h.Handle(It.IsAny<Signal>()), Times.Once);
            _asyncHandler.Verify(h => h.HandleAsync(It.IsAny<Signal>()), Times.Once);
        }

        [Fact]
        public async Task RunAsync_InvalidSignal_NonExecutesAllHandlers()
        {
            var signal = Activator.CreateInstance<Signal>();

            // Act
            await _pipeline.RunAsync(signal);

            // Assert
            _validatorHandler.Verify(h => h.Handle(It.IsAny<Signal>()), Times.Once);
            _syncHandler.Verify(h => h.Handle(It.IsAny<Signal>()), Times.Never);
            _asyncHandler.Verify(h => h.HandleAsync(It.IsAny<Signal>()), Times.Never);
        }

        [Fact]
        public void Constructor_DuplicateFunctionalityHandlers_ThrowsPipelineConfiguratorException()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<Pipeline>>();

            // Act & Assert
            Assert.Throws<PipelineConfiguratorException>(
                () => new Pipeline([new AllHandlers()], loggerMock.Object));

            Assert.Throws<PipelineConfiguratorException>(
                () => new Pipeline([new ValidatorWithBlocking()], loggerMock.Object));

            Assert.Throws<PipelineConfiguratorException>(
                () => new Pipeline([new ValidatorWithAsync()], loggerMock.Object));

            Assert.Throws<PipelineConfiguratorException>(
                () => new Pipeline([new AsyncWithBlocking()], loggerMock.Object));
        }

        class AllHandlers : IValidatorHandler, IBlockingHandler, IAsyncHandler
        {
            public bool Handle(Signal signal) => false;
            public Task HandleAsync(Signal signal) => Task.CompletedTask;
            void IBlockingHandler.Handle(Signal signal) { }
        }

        class ValidatorWithBlocking : IValidatorHandler, IBlockingHandler
        {
            public bool Handle(Signal signal) => false;
            void IBlockingHandler.Handle(Signal signal) { }
        }

        class ValidatorWithAsync : IValidatorHandler, IAsyncHandler
        {
            public bool Handle(Signal signal) => false;
            public Task HandleAsync(Signal signal) => Task.CompletedTask;
        }

        class AsyncWithBlocking : IAsyncHandler, IBlockingHandler
        {
            public void Handle(Signal signal) { }
            public Task HandleAsync(Signal signal) => Task.CompletedTask;
        }
    }
}
