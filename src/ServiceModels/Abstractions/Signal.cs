using MessagePack;

namespace ServiceModels.Abstractions;

[MessagePackObject(keyAsPropertyName: true)]
[GenerateSerializer]
public abstract class Signal<TValue> : BaseSignal
{
    [Id(5)]
    public required TValue Value { get; set; }
}
