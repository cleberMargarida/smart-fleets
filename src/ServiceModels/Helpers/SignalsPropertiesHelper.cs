using ServiceModels.Abstractions;
using ServiceModels.Binding;
using System.Reflection;

namespace ServiceModels.Helpers
{
    internal static class SignalsPropertiesHelper
    {
        internal static Dictionary<SignalType, PropertyInfo> GetProperties<T>()
        {
            return typeof(T)
               .GetProperties(BindingFlags.Instance | BindingFlags.Public)
               .Where(p => p.PropertyType.IsAssignableTo(typeof(BaseSignal)) && p.PropertyType.GetCustomAttribute<TypeIdAttribute>() != null)
               .ToDictionary(p => p.PropertyType.GetCustomAttribute<TypeIdAttribute>()!.Type);
        }
    }
}
