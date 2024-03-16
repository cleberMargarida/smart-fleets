using Mapster;
using MediatR;
using SmartFleets.Domain.Repositories;

namespace SmartFleets.Application.Commands.CreateFaultMetadata
{
    /// <summary>
    /// Represents a handler for the <see cref="CreateFaultMetadataCommand"/>.
    /// </summary>
    public sealed class CreateFaultMetatadaCommandHandler : IRequestHandler<CreateFaultMetadataCommand, CreateFaultMetadataResponse>
    {
        private readonly IFaultMetadataRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateFaultMetatadaCommandHandler"/> class.
        /// </summary>
        /// <param name="repository">The repository for managing fault metadata.</param>
        /// <param name="unitOfWork">The unit of work for managing database transactions.</param>
        public CreateFaultMetatadaCommandHandler(IFaultMetadataRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Handles the creation of fault metadata.
        /// </summary>
        /// <param name="request">The command to create fault metadata.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task representing the asynchronous operation, containing the response with the created fault metadata.</returns>
        public async Task<CreateFaultMetadataResponse> Handle(CreateFaultMetadataCommand request, CancellationToken cancellationToken)
        {
            _repository.Insert(request.FaultMetadata);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return request.FaultMetadata.Adapt<CreateFaultMetadataResponse>();
        }
    }
}
