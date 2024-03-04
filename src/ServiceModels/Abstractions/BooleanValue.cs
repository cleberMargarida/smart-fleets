namespace ServiceModels.Abstractions;

/// <summary>
/// Represents a signal value that is a boolean.
/// </summary>
[GenerateSerializer]
public abstract class BooleanValue : Signal<bool>, ISignalValueAdapter<bool>
{
    /// <summary>
    /// Adapts an object to a boolean value.
    /// </summary>
    /// <param name="source">The object to adapt.</param>
    /// <returns>The adapted boolean value.</returns>
    public bool Adapt(object source)
    {
        return Convert.ToBoolean(source);
    }
}
