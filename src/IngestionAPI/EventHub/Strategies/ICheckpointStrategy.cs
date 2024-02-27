using Azure.Messaging.EventHubs.Processor;

namespace IngestionAPI.EventHub.Strategies;

internal interface ICheckpointStrategy
{
    public Task InvokeAsync(UpdateCheckpointDelegate updateCheckpoint, ProcessEventArgs args, CancellationToken _stoppingToken);
}
