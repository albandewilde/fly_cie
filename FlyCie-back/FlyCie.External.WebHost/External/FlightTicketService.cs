using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FlyCie.External.WebHost.Services
{
    public class FlightTicketService
    {
        private readonly ILogger<FlightTicketService> _logger;
        private readonly ExternalServiceOptions _options;
        private readonly HttpClient _httpClient;

        public FlightTicketService( 
            ILogger<FlightTicketService> logger,
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
                
                foreach( var f in flights )
                {
                    _logger.LogInformation( $"Trying to fetch #{f.code}'s available options" );
                    var res = await _httpClient.GetAsync( $"{_options.ApiUrl}available_options/{f.code}" );
                    var resString = await res.Content.ReadAsStringAsync();
                    var options = JsonSerializer.Deserialize<List<Model.External.FlightOptions>>( resString );
                    f.options = options;
                } 

                return flights;
            }
            catch( Exception e )
            {
                _logger.LogInformation( "An error occurred when fetching external flights" );
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
                    $"{_options.ApiUrl}book",
                    new StringContent( content, Encoding.UTF8, "application/json" )
                );
                if( response.StatusCode == System.Net.HttpStatusCode.OK )
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    _logger.LogInformation( "Successfully booked a ticket." );

                    return ticket;
                }
                else
                {
                    _logger.LogError( "An error occured while trying to book a ticket from External" );

                    return null;
                }
            }
            catch ( Exception e )
            {
                _logger.LogError( "An error occurred while booking a ticket", e );
                return null;
            }
        }
    }

    public sealed class ExternalServiceOptions
    {
        public string ApiUrl { get; set; }
    }
}
