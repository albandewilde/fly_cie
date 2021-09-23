using FlyCie.Model;
using FlyCie.MTD.WebHost.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace FlyCie.MTD.WebHost.Services
{
    public class MTDService
    {
        private readonly MTDServiceOptions _options;
        private readonly ILogger<MTDService> _logger;
        private readonly HttpClient _httpClient;

        public MTDService(
            ILogger<MTDService> logger,
            IOptionsMonitor<MTDServiceOptions> options )
        {
            _logger = logger;
            _options = options.CurrentValue;
            _httpClient = new HttpClient();
        }

        public async Task<IEnumerable<FlightApi>> RequestFlights()
        {
            var requestUrl = $"{_options.ApiUrl}/getAllFlights";

            using ( var requestMessage = new HttpRequestMessage( HttpMethod.Get, requestUrl ) )
            {
                requestMessage.Headers.Add( "ApiKey", _options.ApiKey );
                var response = await _httpClient.SendAsync( requestMessage );
                var resString = await response.Content.ReadAsStringAsync();
                var flights = JsonSerializer.Deserialize<List<MTDFlight>>( resString );

                return flights.Select( f => ModelMapper.MapToFlightApi( f ) );
            }
        }

        public async Task PostBookTicket()
        {

        }
    }

    public sealed class MTDServiceOptions
    {
        public string ApiKey { get; set; }
        public string ApiUrl { get; set; }
    }
}
