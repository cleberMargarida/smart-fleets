using ServiceModels.Binding;
using SmartFleets.Domain.Entities;
using SmartFleets.Domain.Repositories;
using SmartFleets.Infrastructure.Data;

namespace SmartFleets.Infrastructure.Repositories
{
    /// <summary>
    /// Provides a repository for managing fault metadata.
    /// </summary>
    public class FaultMetadataRepository : RepositoryBase<FaultMetadata>, IFaultMetadataRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FaultMetadataRepository"/> class.
        /// </summary>
        /// <param name="context">The application database context.</param>
        public FaultMetadataRepository(ApplicationDbContext context) : base(() => context.FaultMetadatas)
        {
        }

        /// <inheritdoc/>
        public IAsyncEnumerable<FaultMetadata> GetByType(SignalType type)
        {
            return Query(f => f.SignalTypes.Contains(type));
        }
    }
}