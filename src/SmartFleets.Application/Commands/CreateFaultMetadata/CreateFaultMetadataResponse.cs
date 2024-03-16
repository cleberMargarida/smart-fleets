namespace SmartFleets.Application.Commands.CreateFaultMetadata
{
    /// <summary>
    /// Represents the response for creating fault metadata.
    /// </summary>
    /// <param name="Id">The unique identifier of the created fault metadata.</param>
    public record CreateFaultMetadataResponse(Guid Id);
}
