using Azure.Core;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Processor;
using Azure.Storage.Blobs;
using IngestionAPI.EventHub.Strategies;
using System.Reflection;

namespace IngestionAPI.EventHub
{
    public class EventProcessorClientImpl : EventProcessorClient
    {
        private readonly ICheckpointStrategy? _checkpointStrategy;
        private CancellationToken _stoppingToken;

        public EventProcessorClientImpl(
              BlobContainerClient checkpointStore
            , string consumerGroup
            , string connectionString
            , EventProcessorClientOptions clientOptions
            , CheckpointStrategyFactory checkpointStrategyFactory)
            : base(checkpointStore
            , consumerGroup
            , connectionString
            , clientOptions)
        {
            _checkpointStrategy = checkpointStrategyFactory.BuildCheckpointStrategy();
        }

        public EventProcessorClientImpl(
            BlobContainerClient checkpointStore
            , string consumerGroup
            , string fullyQualifiedNamespace
            , string eventHubName
            , TokenCredential credential
            , EventProcessorClientOptions clientOptions
            , CheckpointStrategyFactory checkpointStrategyFactory)
            : base(checkpointStore
            , consumerGroup
            , fullyQualifiedNamespace
            , eventHubName
            , credential
            , clientOptions)
        {
            _checkpointStrategy = checkpointStrategyFactory.BuildCheckpointStrategy();
        }

        public override Task StartProcessingAsync(CancellationToken cancellationToken = default)
        {
            _stoppingToken = cancellationToken;
            WrappProcessEvent();
            return base.StartProcessingAsync(cancellationToken);
        }

        public override void StartProcessing(CancellationToken cancellationToken = default)
        {
            _stoppingToken = cancellationToken;
            WrappProcessEvent();
            base.StartProcessing(cancellationToken);
        }

        private void WrappProcessEvent()
        {
            var processEventField = typeof(EventProcessorClient).GetField("_processEventAsync", BindingFlags.NonPublic | BindingFlags.Instance)
                ?? throw new InvalidOperationException("No _processEventAsync was found to override.");

            var processEventAsync = processEventField.GetValue(this) as Func<ProcessEventArgs, Task>
                ?? throw new InvalidOperationException("No _processEventAsync of type Func<ProcessEventArgs, Task> instance was found.");

            processEventField.SetValue(this, (Func<ProcessEventArgs, Task>)(async args =>
            {
                await RegisterOffsetAsync(args);
                await processEventAsync(args);
            }));
        }

        private async Task RegisterOffsetAsync(ProcessEventArgs args)
        {
            if (_checkpointStrategy is null)
            {
                return;
            }

            await _checkpointStrategy
                .InvokeAsync(() => UpdateCheckpointAsync(args.Partition.PartitionId, args.Data.Offset, null, _stoppingToken), args, _stoppingToken);
        }
    }
}
