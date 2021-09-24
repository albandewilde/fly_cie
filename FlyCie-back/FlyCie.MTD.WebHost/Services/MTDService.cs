using FlyCie.Model;
using FlyCie.Model.MTD;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
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

        public async Task<MTDOrder> PostBookTicket( 
            List<Ticket> tickets, 
            string userType, 
            uint nbAdditionalLuggage )
        {
            try
            {
                var order = CreateMTDOrder( tickets, userType, nbAdditionalLuggage );

                var content = JsonSerializer.Serialize( order );
                var response = await _httpClient.PostAsync(
                    $"{_options.ApiUrl}/BookTicket",
                    new StringContent( content, Encoding.UTF8, "application/json" )
                );
                if ( response.StatusCode == HttpStatusCode.OK )
                {
                    _logger.LogInformation( "Successfully created !" );

                    return order;
                }
                else
                {
                    return null;
                }
            }
            catch( Exception e )
            {
                _logger.LogError( "An error occured while talking to Dan, because Dan is a fdp !", e );
                return null;
            }
        }

        private MTDOrder CreateMTDOrder( List<Ticket> tickets, string userType, uint nbAdditionalLuggage )
        {
            var wantedTickets = new List<MTDTicket>();
            var userName = string.Empty;
            var currency = string.Empty;

            foreach ( var t in tickets )
            {
                var mtdTicket = new MTDTicket
                {
                    FirstName = t.FirstName,
                    IsPaid = false,
                    LastName = t.LastName,
                    UserType = Enum.Parse<UserType>( userType ),
                    NbAdditionalLuggage = nbAdditionalLuggage
                };
                wantedTickets.Add( mtdTicket );
                userName = $"{t.LastName} {t.FirstName}";
                currency = t.Currency.Name;
            }

            return new MTDOrder
            {
                IsPaid = false,
                TicketList = wantedTickets,
                User = new MTDUser { Name = userName },
                UsedCurrency = Enum.Parse<MTDCurrency>( currency )
            };
        }
    }

    public sealed class MTDServiceOptions
    {
        public string ApiKey { get; set; }
        public string ApiUrl { get; set; }
    }
}
