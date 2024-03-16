using SmartFleets.Domain.Entities;
using System.Collections.Generic;

namespace SmartFleets.Domain.Repositories
{
    /// <summary>
    /// Defines the interface for a repository that manages faults.
    /// </summary>
    public interface IFaultRepository : IRepository<Fault>
    {
        /// <summary>
        /// Gets an asynchronous enumerable of today's faults for a specific vehicle.
        /// </summary>
        /// <param name="vehicleId">The identifier of the vehicle.</param>
        /// <returns>An asynchronous enumerable of faults.</returns>
        IAsyncEnumerable<Fault> GetTodays(string vehicleId);
    }
}
