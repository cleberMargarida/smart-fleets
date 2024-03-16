namespace SmartFleets.Domain.Repositories
{
    /// <summary>
    /// Defines the interface for a generic repository.
    /// </summary>
    /// <typeparam name="T">The type of entity managed by the repository.</typeparam>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Inserts multiple entities into the repository.
        /// </summary>
        /// <param name="values">The entities to insert.</param>
        void Insert(IEnumerable<T> values);

        /// <summary>
        /// Inserts a single entity into the repository.
        /// </summary>
        /// <param name="value">The entity to insert.</param>
        void Insert(T value);

        /// <summary>
        /// Removes multiple entities from the repository.
        /// </summary>
        /// <param name="values">The entities to remove.</param>
        void Remove(IEnumerable<T> values);

        /// <summary>
        /// Removes a single entity from the repository.
        /// </summary>
        /// <param name="value">The entity to remove.</param>
        void Remove(T value);

        /// <summary>
        /// Updates multiple entities in the repository.
        /// </summary>
        /// <param name="values">The entities to update.</param>
        void Update(IEnumerable<T> values);

        /// <summary>
        /// Updates a single entity in the repository.
        /// </summary>
        /// <param name="value">The entity to update.</param>
        void Update(T value);
    }
}
