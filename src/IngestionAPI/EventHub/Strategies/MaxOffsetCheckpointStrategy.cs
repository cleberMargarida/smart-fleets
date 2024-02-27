using System.Collections.Concurrent;
using Azure.Messaging.EventHubs.Processor;

namespace IngestionAPI.EventHub.Strategies;

internal class MaxOffsetCheckpointStrategy : ICheckpointStrategy
{
    private const int InitialValue = 1;
    private readonly ConcurrentDictionary<string, long> _offsetByPartition = new();
    private int _maxOffset;

    public MaxOffsetCheckpointStrategy(int maxOffset)
    {
        _maxOffset = maxOffset;
    }

    public async Task InvokeAsync(UpdateCheckpointDelegate updateCheckpoint, ProcessEventArgs args, CancellationToken stoppingToken)
    {
        _offsetByPartition.AddOrUpdate(args.Partition.PartitionId,
                                       InitialValue,
                                       (_, i) => ++i);

        if (_offsetByPartition[args.Partition.PartitionId] <= _maxOffset)
        {
            return;
        }

        _offsetByPartition[args.Partition.PartitionId] = 0;
        await updateCheckpoint();
        return;
    }
}
