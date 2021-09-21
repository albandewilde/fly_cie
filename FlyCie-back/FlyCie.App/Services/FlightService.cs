using FlyCie.App.Abstractions;
using FlyCie.Model;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FlyCie.App.Services
{
    public class FlightService : IHostedService
    {
        private readonly ILogger<FlightService> _logger;

        public FlightService( ILogger<FlightService> logger )
        {
            _logger = logger;
        }

        public async Task StartAsync( CancellationToken cancellationToken )
        {
            _logger.LogInformation( "Initializing possible flights." );

            var flights = new List<Flight>();
            flights.Add( new Flight
            {
                FlightId = 1,
                From = Airport.DTW,
                To = Airport.CDG,
                TotalPlaces = 700,
                AvailablePlaces = 700,
                Price = 700
            } );
            flights.Add( new Flight
            {
                FlightId = 2,
                From = Airport.DTW,
                To = Airport.JFK,
                TotalPlaces = 300,
                AvailablePlaces = 300,
                Price = 300
            } );
            flights.Add( new Flight
            {
                FlightId = 3,
                From = Airport.CDG,
                To = Airport.JFK,
                TotalPlaces = 1000,
                AvailablePlaces = 1000,
                Price = 1000
            } );
            flights.Add( new Flight
            {
                FlightId = 4,
                From = Airport.CDG,
                To = Airport.DTW,
                TotalPlaces = 700,
                AvailablePlaces = 700,
                Price = 700
            } );
            flights.Add( new Flight
            {
                FlightId = 5,
                From = Airport.JFK,
                To = Airport.CDG,
                TotalPlaces = 1000,
                AvailablePlaces = 1000,
                Price = 1000
            } );
            flights.Add( new Flight
            {
                FlightId = 6,
                From = Airport.JFK,
                To = Airport.DTW,
                TotalPlaces = 300,
                AvailablePlaces = 300,
                Price = 300
            } );

            FlightsData.SetFlights( flights );
            _logger.LogInformation( "Flights initialized." );
        }

        public async Task StopAsync( CancellationToken cancellationToken )
        {
            _logger.LogWarning( "Stoping flight service." );
        }
    }
}
