using ServiceModels.Abstractions;
using ServiceModels.Binding;
using System.Reflection;

namespace ServiceModels.Helpers;

/// <summary>
/// Provides a helper for binding signal types to their corresponding classes.
/// </summary>
public static class SignalTypeBindingHelper
{
    private static IReadOnlyDictionary<uint, Type>? _typeMapping;

    /// <summary>
    /// Gets a read-only dictionary that maps signal type numbers to their corresponding signal classes.
    /// </summary>
    public static IReadOnlyDictionary<uint, Type> TypeMapping => _typeMapping ??= Assembly
        .GetExecutingAssembly()
        .DefinedTypes
        .Where(t => typeof(BaseSignal).IsAssignableFrom(t) && !t.IsAbstract)
        .ToDictionary(t => t.GetCustomAttribute<TypeIdAttribute>()!.TypeAsNumber,
                      t => t.AsType())
        .AsReadOnly();
}
