namespace ServiceModels.Abstractions;

/// <summary>
/// Represents a signal value that is a numeric value.
/// </summary>
[GenerateSerializer]
public abstract class Numeric : Signal<double>, ISignalValueAdapter<double>
{
    /// <summary>
    /// Adapts an object to a numeric value.
    /// </summary>
    /// <param name="source">The object to adapt.</param>
    /// <returns>The adapted numeric value.</returns>
    public double Adapt(object source)
    {
        return Convert.ToDouble(source);
    }
}
