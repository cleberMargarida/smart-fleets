using MessagePack;

namespace ServiceModels.Abstractions;

[MessagePackObject(keyAsPropertyName: true)]
public abstract class Signal<TValue> : SignalAbstract
{
    public required TValue Value { get; set; }
}
