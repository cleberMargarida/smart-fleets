using MediatR;
using SmartFleets.Domain.Entities;

namespace SmartFleets.Application.Commands.CreateFault
{
    /// <summary>
    /// Represents a command to create a fault.
    /// </summary>
    /// <param name="Fault">The fault to be created.</param>
    public sealed record CreateFaultCommand(Fault Fault) : IRequest;
}
