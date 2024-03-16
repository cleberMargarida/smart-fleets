using ServiceModels.Binding;

namespace SmartFleets.Application.Commands.CreateFaultMetadata
{
    /// <summary>
    /// Represents a request to create fault metadata.
    /// </summary>
    /// <param name="Description">The description of the fault condition.</param>
    /// <param name="PredicateAsString">
    /// The predicate that defines the fault condition as a string.
    /// <para>Example:</para><code>state.Speed.Value >= 50</code></param>
    /// <param name="SignalTypes">A list of signal types associated with the fault condition.</param>
    /// <param name="Enabled">Indicates whether the fault condition is enabled. Default is true.</param>
    public record CreateFaultMetadataRequest(string Description, string PredicateAsString, IList<SignalType> SignalTypes, bool Enabled = true);
}
