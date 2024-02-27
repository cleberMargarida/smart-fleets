namespace IngestionAPI.EventHub.Strategies;

public class CheckpointStrategyFactory
{
    internal int? MaxOffset { get; private set; }

    internal ICheckpointStrategy? BuildCheckpointStrategy()
    {
        if (MaxOffset.HasValue)
        {
            return new MaxOffsetCheckpointStrategy(MaxOffset.Value);
        }

        return null;
    }

    public void ByCount(int count)
        => MaxOffset = count;
}
