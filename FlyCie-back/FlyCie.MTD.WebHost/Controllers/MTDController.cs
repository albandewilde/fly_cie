using FlyCie.MTD.WebHost.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace FlyCie.MTD.WebHost.Controllers
{
    [Route( "api/mtd" )]
    public class MTDController : Controller
    {
        private readonly MTDService _mtdService;
        private readonly ILogger<MTDController> _logger;

        public MTDController( MTDService mtdService, ILogger<MTDController> logger )
        {
            _mtdService = mtdService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok( "Hello bro, how can I help you ?" );
        }

        [HttpGet( "GetMTDFlights" )]
        public async Task<IActionResult> GetMTDFlights()
        {            
            return Ok( await _mtdService.RequestFlights() );
        }
    }
}
