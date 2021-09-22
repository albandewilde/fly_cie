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
        private readonly ExternalService _externalService;

        public ExternalController( 
            ILogger<ExternalController> logger,
            ExternalService externalService )
        {
            _logger = logger;
            _externalService = externalService;

        }

        [HttpGet( "GetFlights" )]
        public async Task<IActionResult> GetAvailableFlights()
        {
            try
            {
                _logger.LogInformation( $"Fetching external flights" );
                return Ok( await _externalService.GetExternalFlights() );
            }
            catch( Exception e )
            {
                _logger.LogError( $"An error occured while fetching all available flights", e );
                return BadRequest( e );
            }
        }
    }
}
