using SmartFleets.Domain.Entities;
using SmartFleets.Domain.Repositories;
using SmartFleets.Infrastructure.Data;

namespace SmartFleets.Infrastructure.Repositories
{
    /// <summary>
    /// Provides a repository for managing faults.
    /// </summary>
    public class FaultRepository : RepositoryBase<Fault>, IFaultRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FaultRepository"/> class.
        /// </summary>
        /// <param name="context">The application database context.</param>
        public FaultRepository(ApplicationDbContext context) : base(() => context.Faults)
        {
        }

        /// <inheritdoc/>
        public IAsyncEnumerable<Fault> GetTodays(string vehicleId)
        {
            return Query(f => f.CreatedAt.Day == DateTime.Today.Day && f.VehicleId == vehicleId);
        }
    }
}
