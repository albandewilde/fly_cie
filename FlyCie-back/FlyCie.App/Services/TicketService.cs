using FlyCie.App.Abstractions;
using FlyCie.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
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
            foreach( var roundTrip in trips[ "RoundTrips" ] )
            {
                var ticket = await CreateTicket( roundTrip, ticketForm.LastName, ticketForm.FirstName, ticketForm.Nationality, ticketForm.Currency, ticketForm.LoungeSupplement, true );
                result.Add( ticket );
                FlightsData.FlightList.Where( f => f.FlightId == roundTrip.FlightId ).First().AvailablePlaces -= 1;
            }

            foreach( var trip in trips["OneWayTrips"] )
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

        private async Task<Currency> GetCurrency( string currencyName )
        {
            try
            {
                _logger.LogInformation( $"Trying to fetch currency ${currencyName} rate" );
                var response = await _httpClient.GetAsync( _currencyRequestUrl );
                var responseString = response.Content.ReadAsStringAsync();

                return new Currency
                {
                    Name = currencyName,
                    Rate = Convert.ToDouble( responseString )
                };
            }
            catch( Exception e )
            {
                _logger.LogError( "Enable to fetch currency's rate", e );
                throw new Exception( "Enable to fetch currency's rate" );
            }
        }
    }
}
