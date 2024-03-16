using MediatR;
using SmartFleets.Domain.Entities;

namespace SmartFleets.Application.Commands.CreateFaultMetadata
{
    /// <summary>
    /// Represents a command to create fault metadata.
    /// </summary>
    /// <param name="FaultMetadata">The fault metadata to be created.</param>
    public sealed record CreateFaultMetadataCommand(FaultMetadata FaultMetadata) : IRequest<CreateFaultMetadataResponse>;
}
