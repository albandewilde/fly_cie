using FlyCie.App.Helpers;
using FlyCie.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FlyCie.App.Services
{
    public class ExternalService
    {
        private readonly ILogger<ExternalService> _logger;
        private readonly ExternalApiOptions _options;
        private readonly HttpClient _httpClient;

        public ExternalService( 
            ILogger<ExternalService> logger,
            IOptionsMonitor<ExternalApiOptions> options )
        {
            _options = options.CurrentValue;
            _httpClient = new HttpClient();
            _logger = logger;
        }

        public async Task<IEnumerable<FlightApi>> GetExternalFlights()
        {
            try
            {
                var response = await _httpClient.GetAsync( $"{_options.ExternalApiUrl}/api/external/GetFlights" );
                var responseString = await response.Content.ReadAsStringAsync();
                var externalFlights = JsonSerializer.Deserialize<List<Model.External.Flight>>( responseString );
                
                return from ef in externalFlights select ExternalModelHelper.MapFlightApi( ef );
            }
            catch ( Exception e )
            {
                _logger.LogError( "WTF Something happened when fetching external flights !", e );
                return null;
            }
        }

        public async Task<Model.External.Ticket> SendBookTicket( Model.External.Ticket ticket )
        {
            try
            {
                _logger.LogInformation( $"Trying to send request to book ticket" );
                var content = JsonSerializer.Serialize( ticket );
                var response = await _httpClient.PostAsync(
                    $"{_options.ExternalApiUrl}/api/external/BookTicket",
                    new StringContent( content, Encoding.UTF8, "application/json" )
                );
                var responseString = await response.Content.ReadAsStringAsync();
                _logger.LogInformation( "Successfully booked a ticket." );

                return JsonSerializer.Deserialize<Model.External.Ticket>( responseString );
            }
            catch ( Exception e )
            {
                _logger.LogError( "An error occurred while booking a ticket", e );
                return null;
            }
        }
    }
    public sealed class ExternalApiOptions
    {
        public string ExternalApiUrl { get; set; }
    }
}

