using AutoMapper;
using IngestionAPI.Models;
using ServiceModels;

namespace IngestionAPI.EventHub.MappingProfile
{
    public class SignalsMappingProfile : Profile
    {
        public SignalsMappingProfile()
        {
            var sourceType = typeof(Signal);

            foreach (var (id, destinationType) in SignalTypeBinding.TypeMapping)
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
}
