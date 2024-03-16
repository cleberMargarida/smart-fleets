using SmartFleets.Domain.Repositories;
using SmartFleets.Infrastructure.Data;

namespace SmartFleets.Infrastructure.Repositories
{
    /// <summary>
    /// Represents a unit of work that encapsulates the transaction boundary for a series of data changes.
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
        /// </summary>
        /// <param name="context">The application database context.</param>
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc/>
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }
    }
}