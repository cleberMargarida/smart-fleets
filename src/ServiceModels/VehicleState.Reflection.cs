using ServiceModels.Abstractions;
using ServiceModels.Binding;
using ServiceModels.Helpers;
using System.Reflection;

namespace ServiceModels;

public partial class VehicleState
{
    private static readonly Dictionary<SignalType, PropertyInfo> _propertyByType
        = SignalsPropertiesHelper.GetProperties<VehicleState>();

    public BaseSignal this[SignalType type]
    {
        get => (BaseSignal)_propertyByType[type].GetValue(this)!;
        set => _propertyByType[type].SetValue(this, value);
    }
}
