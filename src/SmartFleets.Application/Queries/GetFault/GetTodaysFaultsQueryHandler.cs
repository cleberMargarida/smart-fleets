using MediatR;
using SmartFleets.Domain.Entities;
using SmartFleets.Domain.Repositories;

namespace SmartFleets.Application.Queries.GetFault
{
    /// <summary>
    /// Represents a handler for the <see cref="GetTodaysFaultsQuery"/>.
    /// </summary>
    public class GetTodaysFaultsQueryHandler : IRequestHandler<GetTodaysFaultsQuery, IReadOnlyCollection<Fault>>
    {
        private readonly IFaultRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetTodaysFaultsQueryHandler"/> class.
        /// </summary>
        /// <param name="repository">The repository for managing faults.</param>
        public GetTodaysFaultsQueryHandler(IFaultRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Handles the query to get today's faults for a specific vehicle.
        /// </summary>
        /// <param name="request">The query request.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task representing the asynchronous operation, containing the collection of today's faults.</returns>
        public async Task<IReadOnlyCollection<Fault>> Handle(GetTodaysFaultsQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetTodays(request.VehicleId).ToListAsync(cancellationToken);
        }
    }
}
