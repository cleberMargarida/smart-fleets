using ServiceModels.Binding;
using SmartFleets.Domain.Entities;
using System.Collections.Generic;

namespace SmartFleets.Domain.Repositories
{
    /// <summary>
    /// Defines the interface for a repository that manages fault metadata.
    /// </summary>
    public interface IFaultMetadataRepository : IRepository<FaultMetadata>
    {
        /// <summary>
        /// Gets an asynchronous enumerable of fault metadata by signal type.
        /// </summary>
        /// <param name="type">The signal type to filter by.</param>
        /// <returns>An asynchronous enumerable of fault metadata.</returns>
        IAsyncEnumerable<FaultMetadata> GetByType(SignalType type);
    }
}
