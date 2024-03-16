using MediatR;
using ServiceModels.Binding;
using SmartFleets.Domain.Entities;

namespace SmartFleets.Application.Queries.GetFaultMetadata
{
    /// <summary>
    /// Represents a query to get fault metadata by signal type.
    /// </summary>
    /// <param name="Type">The signal type for which fault metadata is requested.</param>
    public record class GetFaultMetadataBySignalTypeQuery(SignalType Type) : IRequest<List<FaultMetadata>>;
}
