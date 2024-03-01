using AutoMapper;
using ServiceModels;
using ServiceModels.Helpers;

namespace IngestionAPI.Models;

public class SignalMappingProfile : Profile
{
    public SignalMappingProfile()
    {
        var sourceType = typeof(Signal);

        foreach (var (id, destinationType) in SignalTypeBindingHelper.TypeMapping)
        {
            CreateMap(sourceType, destinationType)
                .ForMember("Value", cfg => cfg.MapFrom<ValueResolver>());
        }
    }
}

class ValueResolver : IValueResolver<object, object, object>
{
    public object Resolve(object source, object destination, object destMember, ResolutionContext context)
    {
        if (source is not Signal signal)
        {
            throw ConversionError(source, destination, destMember);
        }

        if (destination is not ISignalValueAdapter adapter)
        {
            throw ConversionError(source, destination, destMember);
        }

        var value = signal.Value as object
            ?? signal.Values
            ?? throw new NullReferenceException();

        return adapter.Adapt(value);
    }

    private static NotImplementedException ConversionError(object source, object destination, object destMember)
    {
        return new NotImplementedException(
            $"Not implemented conversion between " +
            $"source:{source.GetType()}, " +
            $"destination:{destination.GetType()}, " +
            $"destMember:{destMember.GetType()}");
    }
}
