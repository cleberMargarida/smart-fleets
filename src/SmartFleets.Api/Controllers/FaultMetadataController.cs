using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartFleets.Application.Commands.CreateFaultMetadata;
using Mapster;

namespace SmartFleets.Api.Controllers
{
    /// <summary>
    /// Controller for managing fault metadata.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class FaultMetadataController : ControllerBase
    {
        private readonly ISender _sender;

        /// <summary>
        /// Initializes a new instance of the <see cref="FaultMetadataController"/> class.
        /// </summary>
        /// <param name="sender">The mediator for sending commands.</param>
        public FaultMetadataController(ISender sender)
        {
            _sender = sender;
        }

        /// <summary>
        /// Creates fault metadata.
        /// </summary>
        /// <param name="request">The fault metadata creation request.</param>
        /// <param name="cancellationToken">Cancellation token for the request.</param>
        /// <returns>A response with the created fault metadata.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(CreateFaultMetadataResponse), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateFaultMetadata([FromBody] CreateFaultMetadataRequest request, CancellationToken cancellationToken)
        {
            var command = request.Adapt<CreateFaultMetadataCommand>();
            var response = await _sender.Send(command, cancellationToken);
            return CreatedAtAction(nameof(CreateFaultMetadata), response);
        }
    }
}
