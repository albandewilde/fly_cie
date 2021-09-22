using FlyCie.App.Abstractions;
using FlyCie.App.Services;
using FlyCie.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FlyCie.App.Controllers
{
    [Route("api/flight")]
    public class FlightController: Controller
    {
        private readonly ILogger<FlightController> _logger;
        private readonly ITicketService _ticketService;

        public FlightController( 
            ILogger<FlightController> logger, 
            ITicketService ticketService )
        {
            _logger = logger;
            _ticketService = ticketService;
        }

        [HttpGet]
        public ActionResult Get()
        {
            _logger.LogInformation( "Requesting IsAlive endpoint" );
            return Ok( "Fly Cie, this is Pam, how may I help you ?" );
        }

        [HttpGet( "GetFlights" )]
        public async Task<IActionResult> GetFlights()
        {
            _logger.LogInformation( "Trying to fetch all available flights." );

            return Ok( FlightsData.GetAvailableFlights() );
        }

        [HttpPost( "BookTicket" )]
        public async Task<IActionResult> BookTicket( [FromBody]TicketForm ticketForm )
        {
            try
            {
                if ( string.IsNullOrWhiteSpace( ticketForm.Currency )
                    || string.IsNullOrWhiteSpace( ticketForm.FirstName )
                    || string.IsNullOrWhiteSpace( ticketForm.LastName )
                    || string.IsNullOrWhiteSpace( ticketForm.Nationality )
                    || ticketForm.FlightIds.ToList().Count <= 0 )
                {
                    _logger.LogError( "Form contains parameters that don't match requirements" );
                    return BadRequest( "Form contains parameters thatmatch requirements" );
                }

                _logger.LogInformation( 
                    $"Trying to book tickets for user: {ticketForm.LastName} {ticketForm.FirstName}" 
                );

                var result = await _ticketService.BookTickets( ticketForm );

                _logger.LogInformation( "Creating tickets was succefully executed" );
                return Ok( result );
            }
            catch( Exception e )
            {
                return BadRequest( e );
            }
        }

        [HttpGet( "Currencies" )]
        public async Task<IActionResult> GetCurrencies()
        {
            _logger.LogInformation( $"Trying to fetch all currencies" );
            return Ok( await _ticketService.GetCurrencies() );
        }

        [HttpGet( "GetRate" )]
        public async Task<IActionResult> GetRate( [FromQuery] string currency )
        {
            _logger.LogInformation( $"Trying to fetch currency {currency}" );
            var res = await _ticketService.GetCurrency( currency );
            return Ok( res.Rate );
        }
    }
}
