using ServiceModels.Abstractions;
using ServiceModels.Binding;
using System.Reflection;

namespace ServiceModels.Helpers;

public static class SignalTypeBindingHelper
{
    private static IReadOnlyDictionary<uint, Type>? _typeMapping;
    public static IReadOnlyDictionary<uint, Type> TypeMapping => _typeMapping ??= Assembly
        .GetExecutingAssembly()
        .DefinedTypes
        .Where(t => typeof(BaseSignal).IsAssignableFrom(t) && !t.IsAbstract)
        .ToDictionary(t => t.GetCustomAttribute<TypeIdAttribute>()!.TypeAsNumber,
                        t => t.AsType())
        .AsReadOnly();
}