using ServiceModels.Abstractions;

namespace ServiceModels.Binding;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class TypeIdAttribute : Attribute
{
    private readonly SignalType _type;

    public TypeIdAttribute(SignalType type)
    {
        _type = type;
    }

    public SignalType Type => _type;
    public uint TypeAsNumber => (uint)_type;
}

[Serializable]
public class TypeIdNotDecoratedException : Exception
{
    public TypeIdNotDecoratedException(Type type) : base($"The class of type {type} must be decorated with TypeIdAttribute.") { }
    public static TypeIdNotDecoratedException NotDecorated<T>() where T : BaseSignal => new TypeIdNotDecoratedException(typeof(T));
    public static TypeIdNotDecoratedException NotDecorated(Type type) => new TypeIdNotDecoratedException(type);
}
