using FlyCie.App.Abstractions;
using FlyCie.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace FlyCie.App.Services
{
    public class TicketService : ITicketService
    {
        private readonly ILogger<TicketService> _logger;
        private readonly HttpClient _httpClient;
        private readonly string _currencyRequestUrl;

        public TicketService( ILogger<TicketService> logger )
        {
            _logger = logger;
            _httpClient = new HttpClient();
            _currencyRequestUrl = "http://localhost:7861/";
        }

        public async Task<List<Ticket>> BookTickets( TicketForm ticketForm )
        {
            foreach( var id in ticketForm.FlightIds )
            {
                if( !FlightsData.HasAvailablePlace( id ) )
                {
                    _logger.LogError( $"Flight #{id} has no available places" );
                 
                    throw new Exception( $"Flight #{id} has no available places" );
                }
            }

            var trips = FlightsData.GetRoundTrips( ticketForm.FlightIds.ToList() );

            var result = new List<Ticket>();
            foreach( var ( roundTrip, index ) in trips[ "RoundTrips" ].Select( (t, index) => (t, index) ) )
            {
                var supplement = false;
                if( ticketForm.LoungeSupplement )
                {
                    if( index % 2 == 0 )
                    {
                        supplement = true;
                    }
                }

                var ticket = await CreateTicket( roundTrip, ticketForm.LastName, ticketForm.FirstName, ticketForm.Nationality, ticketForm.Currency, supplement, true );
                result.Add( ticket );
                FlightsData.FlightList.Where( f => f.FlightId == roundTrip.FlightId ).First().AvailablePlaces -= 1;
            }

            foreach( var (trip, index) in trips["OneWayTrips"].Select( ( t, index ) => (t, index) ) )
            {
                var ticket = await CreateTicket( trip, ticketForm.LastName, ticketForm.FirstName, ticketForm.Nationality, ticketForm.Currency, ticketForm.LoungeSupplement, false );
                result.Add( ticket );
                FlightsData.FlightList.Where( f => f.FlightId == trip.FlightId ).First().AvailablePlaces -= 1;
            }

            return result;
        }

        private async Task<Ticket> CreateTicket( Flight flight, string lastName, string firstName, string nat, string currencyName, bool loungeSupplement, bool isRoundTrip )
        {
            var price = isRoundTrip ? flight.Price * 0.9 : flight.Price;
            if( flight.From != Airport.DTW )
            {
                if( loungeSupplement )
                {
                    price += 150;
                }
            }

            var currency = await GetCurrency( currencyName );

            return new Ticket
            {
                Currency = currency,
                FirstName = firstName,
                LastName = lastName,
                Date = DateTime.UtcNow,
                Flight = flight,
                LoungeSupplement = loungeSupplement,
                Nationality = nat,
                Price = price
            };
        }

        public async Task<Currency> GetCurrency( string currencyName )
        {
            try
            {
                _logger.LogInformation( $"Trying to fetch currency ${currencyName} rate" );
                var requestUrl = $"{_currencyRequestUrl}{currencyName}";
                _logger.LogInformation( $"Requesting : {requestUrl}" );

                var response = await _httpClient.GetAsync( requestUrl );
                var responseString = await response.Content.ReadAsStringAsync();

                return new Currency
                {
                    Name = currencyName,
                    Rate = Convert.ToDouble( responseString, CultureInfo.InvariantCulture )
                };
            }
            catch( Exception e )
            {
                _logger.LogError( "Unable to fetch currency's rate", e );
                throw new Exception( "Unable to fetch currency's rate" );
            }
        }

        public async Task<string> GetCurrencies()
        {
            try
            {
                _logger.LogInformation( $"Trying to fetch currencies" );
                var requestUrl = $"{_currencyRequestUrl}currencies";
                _logger.LogInformation( $"Requesting : {requestUrl}" );

                var response = await _httpClient.GetAsync( requestUrl );
                var responseString = await response.Content.ReadAsStringAsync();

                return responseString;
            }
            catch ( Exception e )
            {
                _logger.LogError( "Enable to fetch currency's rate", e );
                return null;
            }
        }
    }
}
