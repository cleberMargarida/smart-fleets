using MediatR;
using SmartFleets.Domain.Entities;

namespace SmartFleets.Application.Queries.GetFault
{
    /// <summary>
    /// Represents a query to get today's faults for a specific vehicle.
    /// </summary>
    /// <param name="VehicleId">The vehicle identifier.</param>
    public record GetTodaysFaultsQuery(string VehicleId) : IRequest<IReadOnlyCollection<Fault>>;
}
