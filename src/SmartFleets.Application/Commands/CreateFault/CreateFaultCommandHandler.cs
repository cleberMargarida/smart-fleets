using MediatR;
using SmartFleets.Domain.Repositories;

namespace SmartFleets.Application.Commands.CreateFault
{
    /// <summary>
    /// Represents a handler for the <see cref="CreateFaultCommand"/>.
    /// </summary>
    public sealed class CreateFaultCommandHandler : IRequestHandler<CreateFaultCommand>
    {
        private readonly IFaultRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateFaultCommandHandler"/> class.
        /// </summary>
        /// <param name="repository">The repository for managing faults.</param>
        public CreateFaultCommandHandler(IFaultRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Handles the creation of a fault.
        /// </summary>
        /// <param name="request">The command to create a fault.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task Handle(CreateFaultCommand request, CancellationToken cancellationToken)
        {
            _repository.Insert(request.Fault);
            return Task.CompletedTask;
        }
    }
}
