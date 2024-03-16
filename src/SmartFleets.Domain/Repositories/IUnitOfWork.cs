namespace SmartFleets.Domain.Repositories
{
    /// <summary>
    /// Defines the interface for a unit of work, which is used to manage transactions and persist changes to the database.
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// Saves all changes made in this unit of work asynchronously.
        /// </summary>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous save operation. The task result contains the number of objects written to the underlying database.</returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
