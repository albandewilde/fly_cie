﻿using FlyCie.App.Abstractions;
using FlyCie.App.Helpers;
using FlyCie.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        private readonly List<Ticket> _tickets;
        private readonly QueueService _queueService;

        public TicketService( ILogger<TicketService> logger, QueueService queueService )
        {
            _logger = logger;
            _httpClient = new HttpClient();
            _currencyRequestUrl = "http://currency:7861/";
            _tickets = new List<Ticket>();
            _queueService = queueService;
        }

        private async Task<List<Ticket>> BookTickets( TicketForm ticketForm )
        {
            foreach( var order in ticketForm.FlightCodes )
            {
                if( !FlightsData.HasAvailablePlace( order.Code, ticketForm.SelectedDate ) )
                {
                    _logger.LogError( $"Flight #{order} has no available places" );                 
                    return null;
                }
            }

            var trips = FlightsData.GetRoundTrips( ticketForm.FlightCodes.Select( f => f.Code ).ToList() );

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
                FlightsData.FlightList.Where( f => f.flightCode == roundTrip.flightCode ).First().availablePlaces -= 1;
            }

            foreach( var (trip, index) in trips["OneWayTrips"].Select( ( t, index ) => (t, index) ) )
            {
                var ticket = await CreateTicket( trip, ticketForm.LastName, ticketForm.FirstName, ticketForm.Nationality, ticketForm.Currency, ticketForm.LoungeSupplement, false );
                result.Add( ticket );
                FlightsData.FlightList.Where( f => f.flightCode == trip.flightCode ).First().availablePlaces -= 1;
            }
            _tickets.AddRange( result );
            return result;
        }

        private async Task<Ticket> CreateTicket( Flight flight, string lastName, string firstName, string nat, string currencyName, bool loungeSupplement, bool isRoundTrip )
        {
            var price = isRoundTrip ? flight.price * 0.9 : flight.price;
            if( Enum.Parse<Airport>( flight.from ) != Airport.DTW )
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

        public async Task<List<Ticket>> HandleBook( TicketForm ticketForm )
        {
            List<Ticket> tickets = new List<Ticket>();
            TicketForm ourFlightsIds = new TicketForm
            {
                FirstName = ticketForm.FirstName,
                LastName = ticketForm.LastName,
                Nationality = ticketForm.Nationality,
                LoungeSupplement = ticketForm.LoungeSupplement,
                FlightCodes = new List<FlightOrder>(),
                Currency = ticketForm.Currency,
                SelectedDate = ticketForm.SelectedDate
            };
           
            foreach (var order in ticketForm.FlightCodes)
            {
                if ( FlightsData.IsOurTrip( order.Code ) )
                {
                    ourFlightsIds.FlightCodes = ourFlightsIds.FlightCodes.Append( order );
                }
                else
                {
                    var flight = FlightsData.GetFlight( order.Code, false );
                    Ticket ticket = new Ticket
                    {
                        Flight = flight,
                        Date = ticketForm.SelectedDate,
                        Price = flight.price,
                        FirstName = ticketForm.FirstName,
                        LastName = ticketForm.LastName,
                        Nationality = ticketForm.Nationality,
                        LoungeSupplement = ticketForm.LoungeSupplement,
                        Currency = await GetCurrency( ticketForm.Currency )
                    };

                    var extTicket = ExternalModelHelper.MapTicket( ticket );

                    _queueService.EnqueueTicket( extTicket );
                }
            }
            if( ourFlightsIds.FlightCodes.Count() > 0 )
            {
                tickets.AddRange( await BookTickets( ourFlightsIds ) );
            }
            
            return tickets;
        }
    }
}
