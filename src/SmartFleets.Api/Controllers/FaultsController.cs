using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartFleets.Application.Queries.GetFault;
using SmartFleets.Domain.Entities;

namespace SmartFleets.Api.Controllers
{
    /// <summary>
    /// Controller for managing faults.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class FaultsController : ControllerBase
    {
        private readonly ISender _sender;

        /// <summary>
        /// Initializes a new instance of the <see cref="FaultsController"/> class.
        /// </summary>
        /// <param name="sender">The mediator for sending queries.</param>
        public FaultsController(ISender sender)
        {
            _sender = sender;
        }

        /// <summary>
        /// Retrieves today's faults for a specific vehicle.
        /// </summary>
        /// <param name="vehicleId">The vehicle ID to retrieve faults for.</param>
        /// <param name="cancellationToken">Cancellation token for the request.</param>
        /// <returns>A collection of today's faults for the specified vehicle.</returns>
        [HttpGet("todays/{vehicleId}")]
        [ProducesResponseType(typeof(IReadOnlyCollection<Fault>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTodays(string vehicleId, CancellationToken cancellationToken)
        {
            var query = new GetTodaysFaultsQuery(vehicleId);
            var faults = await _sender.Send(query, cancellationToken);
            return Ok(faults);
        }
    }
}
