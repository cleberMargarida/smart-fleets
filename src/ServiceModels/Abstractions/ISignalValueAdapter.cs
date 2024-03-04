namespace ServiceModels;

/// <summary>
/// Defines a method for adapting an object to a specific signal value type.
/// </summary>
public interface ISignalValueAdapter
{
    /// <summary>
    /// Adapts an object to a specific signal value type.
    /// </summary>
    /// <param name="source">The object to adapt.</param>
    /// <returns>The adapted signal value.</returns>
    object Adapt(object source);
}

/// <summary>
/// Defines a method for adapting an object to a specific signal value type.
/// </summary>
/// <typeparam name="T">The type of the adapted signal value.</typeparam>
public interface ISignalValueAdapter<out T> : ISignalValueAdapter
{
    /// <summary>
    /// Adapts an object to a specific signal value type.
    /// </summary>
    /// <param name="source">The object to adapt.</param>
    /// <returns>The adapted signal value of type <typeparamref name="T"/>.</returns>
    new T Adapt(object source);

    /// <summary>
    /// Adapts an object to a specific signal value type. This method is an explicit interface implementation
    /// to satisfy the <see cref="ISignalValueAdapter"/> interface.
    /// </summary>
    /// <param name="source">The object to adapt.</param>
    /// <returns>The adapted signal value.</returns>
#pragma warning disable CS8603
    object ISignalValueAdapter.Adapt(object source) => Adapt(source);
#pragma warning restore CS8603
}
