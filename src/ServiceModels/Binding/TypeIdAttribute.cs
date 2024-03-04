using ServiceModels.Abstractions;

namespace ServiceModels.Binding;

/// <summary>
/// Attribute used to associate a signal class with a specific signal type.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class TypeIdAttribute : Attribute
{
    private readonly SignalType _type;

    /// <summary>
    /// Initializes a new instance of the <see cref="TypeIdAttribute"/> class.
    /// </summary>
    /// <param name="type">The signal type to associate with the class.</param>
    public TypeIdAttribute(SignalType type)
    {
        _type = type;
    }

    /// <summary>
    /// Gets the signal type associated with the class.
    /// </summary>
    public SignalType Type => _type;

    /// <summary>
    /// Gets the numeric value of the signal type associated with the class.
    /// </summary>
    public uint TypeAsNumber => (uint)_type;
}

/// <summary>
/// Exception thrown when a signal class is not decorated with the <see cref="TypeIdAttribute"/>.
/// </summary>
[Serializable]
[GenerateSerializer]
public class TypeIdNotDecoratedException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeIdNotDecoratedException"/> class.
    /// </summary>
    /// <param name="type">The type of the class that is missing the attribute.</param>
    public TypeIdNotDecoratedException(Type type) : base($"The class of type {type} must be decorated with TypeIdAttribute.") { }

    /// <summary>
    /// Creates a new instance of the <see cref="TypeIdNotDecoratedException"/> class for a specific signal type.
    /// </summary>
    /// <typeparam name="T">The signal type that is missing the attribute.</typeparam>
    /// <returns>A new instance of the exception.</returns>
    public static TypeIdNotDecoratedException NotDecorated<T>() where T : BaseSignal => new TypeIdNotDecoratedException(typeof(T));

    /// <summary>
    /// Creates a new instance of the <see cref="TypeIdNotDecoratedException"/> class for a specific type.
    /// </summary>
    /// <param name="type">The type that is missing the attribute.</param>
    /// <returns>A new instance of the exception.</returns>
    public static TypeIdNotDecoratedException NotDecorated(Type type) => new TypeIdNotDecoratedException(type);
}
