using Mapster;
using SmartFleets.Application.Commands.CreateFault;
using SmartFleets.Application.Commands.CreateFaultMetadata;
using SmartFleets.Domain.Entities;

namespace SmartFleets.Application
{
    /// <summary>
    /// Configures mapping between domain entities and application commands using Mapster.
    /// </summary>
    public static class MappingConfiguration
    {
        /// <summary>
        /// Applies the mapping configuration.
        /// </summary>
        public static void Apply()
        {
            /* Configure classes mapping here. */
            TypeAdapterConfig globalSettings = TypeAdapterConfig.GlobalSettings;

            globalSettings.ForType<Fault, CreateFaultCommand>()
                          .Map(dest => dest.Fault, src => src);

            globalSettings.ForType<CreateFaultMetadataRequest, CreateFaultMetadataCommand>()
                          .Map(dest => dest.FaultMetadata, src => src);

            globalSettings.ForType<CreateFaultMetadataCommand, CreateFaultMetadataResponse>()
                          .Map(dest => dest, src => src.FaultMetadata);
        }
    }
}
