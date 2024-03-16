using Microsoft.EntityFrameworkCore;
using SmartFleets.Domain.Repositories;
using System.Linq.Expressions;

namespace SmartFleets.Infrastructure.Repositories
{
    /// <summary>
    /// Provides a base implementation for a repository.
    /// </summary>
    /// <typeparam name="T">The type of the entity managed by the repository.</typeparam>
    public abstract class RepositoryBase<T> : IRepository<T> where T : class
    {
        private readonly DbSet<T> _set;

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryBase{T}"/> class.
        /// </summary>
        /// <param name="dbSetFactory">A factory function to create the <see cref="DbSet{T}"/>.</param>
        public RepositoryBase(Func<DbSet<T>> dbSetFactory)
        {
            _set = dbSetFactory();
        }

        /// <summary>
        /// Queries the dataset.
        /// </summary>
        /// <param name="predicate">The expression used to filter the dataset.</param>
        /// <returns>An asynchronous enumerable of entities that satisfy the predicate.</returns>
        protected IAsyncEnumerable<T> Query(Expression<Func<T, bool>> predicate)
        {
            return _set.Where(predicate).AsAsyncEnumerable();
        }

        /// <inheritdoc/>
        public void Insert(T value)
        {
            _set.Add(value);
        }

        /// <inheritdoc/>
        public void Insert(IEnumerable<T> values)
        {
            _set.AddRange(values);
        }

        /// <inheritdoc/>
        public void Update(T value)
        {
            _set.Update(value);
        }

        /// <inheritdoc/>
        public void Update(IEnumerable<T> values)
        {
            _set.UpdateRange(values);
        }

        /// <inheritdoc/>
        public void Remove(T value)
        {
            _set.Remove(value);
        }

        /// <inheritdoc/>
        public void Remove(IEnumerable<T> values)
        {
            _set.RemoveRange(values);
        }
    }
}
