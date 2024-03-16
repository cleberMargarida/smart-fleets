using MediatR;
using SmartFleets.Domain.Entities;
using SmartFleets.Domain.Repositories;

namespace SmartFleets.Application.Queries.GetFaultMetadata
{
    /// <summary>
    /// Represents a handler for the <see cref="GetFaultMetadataBySignalTypeQuery"/>.
    /// </summary>
    public class GetFaultMetadatasBySignalTypeQueryHandler : IRequestHandler<GetFaultMetadataBySignalTypeQuery, List<FaultMetadata>>
    {
        private readonly IFaultMetadataRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetFaultMetadatasBySignalTypeQueryHandler"/> class.
        /// </summary>
        /// <param name="repository">The repository for managing fault metadata.</param>
        public GetFaultMetadatasBySignalTypeQueryHandler(IFaultMetadataRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Handles the query to get fault metadata by signal type.
        /// </summary>
        /// <param name="request">The query request.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task representing the asynchronous operation, containing the list of fault metadata.</returns>
        public async Task<List<FaultMetadata>> Handle(GetFaultMetadataBySignalTypeQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetByType(request.Type).ToListAsync(cancellationToken);
        }
    }
}
