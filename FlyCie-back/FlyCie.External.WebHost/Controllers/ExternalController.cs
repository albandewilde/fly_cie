using FlyCie.External.WebHost.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FlyCie.External.WebHost.Controllers
{
    [Route( "api/external" )]
    public class ExternalController : Controller
    {
        private readonly ILogger<ExternalController> _logger;
        private readonly FlightTicketService _externalTicketService;

        public ExternalController( 
            ILogger<ExternalController> logger,
            FlightTicketService externalService )
        {
            _logger = logger;
            _externalTicketService = externalService;

        }

        [HttpGet( "GetFlights" )]
        public async Task<IActionResult> GetAvailableFlights()
        {
            try
            {
                _logger.LogInformation( $"Fetching external flights" );
                return Ok( await _externalTicketService.GetExternalFlights() );
            }
            catch( Exception e )
            {
                _logger.LogError( $"An error occured while fetching all available flights", e );
                return BadRequest( e );
            }
        }

        [HttpPost( "BookTicket" )]
        public async Task<IActionResult> BookTicket( [FromBody] Model.External.Ticket ticket )
        {
            try
            {
                _logger.LogInformation( $"Trying to book a ticket" );
                return Ok( await _externalTicketService.SendBookTicket( ticket ) );
            }
            catch ( Exception e )
            {
                _logger.LogError( $"An error occured while creating a ticket", e );
                return BadRequest( e );
            }
        }
    }
}
