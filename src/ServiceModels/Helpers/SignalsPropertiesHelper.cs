using ServiceModels.Abstractions;
using ServiceModels.Binding;
using System.Reflection;

namespace ServiceModels.Helpers
{
    /// <summary>
    /// Provides helper methods for working with signal properties.
    /// </summary>
    internal static class SignalsPropertiesHelper
    {
        /// <summary>
        /// Retrieves a dictionary of signal types and their corresponding properties for a given type.
        /// </summary>
        /// <typeparam name="T">The type to inspect for signal properties.</typeparam>
        /// <returns>A dictionary mapping signal types to their corresponding properties.</returns>
        internal static Dictionary<SignalType, PropertyInfo> GetProperties<T>()
        {
            return typeof(T)
               .GetProperties(BindingFlags.Instance | BindingFlags.Public)
               .Where(p => p.PropertyType.IsAssignableTo(typeof(BaseSignal)) && p.PropertyType.GetCustomAttribute<TypeIdAttribute>() != null)
               .ToDictionary(p => p.PropertyType.GetCustomAttribute<TypeIdAttribute>()!.Type);
        }
    }
}
