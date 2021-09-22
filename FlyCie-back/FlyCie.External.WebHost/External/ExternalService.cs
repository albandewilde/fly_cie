using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace FlyCie.External.WebHost.Services
{
    public class ExternalService
    {
        private readonly ILogger<ExternalService> _logger;
        private readonly ExternalServiceOptions _options;
        private readonly HttpClient _httpClient;

        public ExternalService( 
            ILogger<ExternalService> logger,
            IOptionsMonitor<ExternalServiceOptions> options )
        {
            _logger = logger;
            _options = options.CurrentValue;
            _httpClient = new HttpClient();
        }

        public async Task<IEnumerable<Model.External.Flight>> GetExternalFlights()
        {
            try
            {
                _logger.LogInformation( $"Trying to fetch external's flights" );
                var response = await _httpClient.GetAsync( $"{_options.ApiUrl}flights" );
                var responseString = await response.Content.ReadAsStringAsync();
                var flights = JsonSerializer.Deserialize<List<Model.External.Flight>>( responseString );
            
                return flights;
            }
            catch( Exception e )
            {
                _logger.LogInformation( "An error occurred when fetching external flights" );
                return null;
            }
        }
    }

    public sealed class ExternalServiceOptions
    {
        public string ApiUrl { get; set; }
    }
}
