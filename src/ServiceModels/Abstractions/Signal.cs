using MessagePack;

namespace ServiceModels.Abstractions;

/// <summary>
/// Represents a generic signal with a specific value type.
/// </summary>
/// <typeparam name="TValue">The type of the signal value.</typeparam>
[MessagePackObject(keyAsPropertyName: true)]
[GenerateSerializer]
public abstract class Signal<TValue> : BaseSignal
{
    /// <summary>
    /// Gets or sets the value of the signal.
    /// </summary>
    [Id(5)]
    public required TValue Value { get; set; }
}
