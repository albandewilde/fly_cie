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
                FlightId = "DT3712",
                From = Airport.DTW,
                To = Airport.CDG,
                TotalPlaces = 700,
                AvailablePlaces = 700,
                Price = 700
            } );
            flights.Add( new Flight
            {
                FlightId = "DT3333",
                From = Airport.DTW,
                To = Airport.JFK,
                TotalPlaces = 300,
                AvailablePlaces = 300,
                Price = 300
            } );
            flights.Add( new Flight
            {
                FlightId = "AF2458",
                From = Airport.CDG,
                To = Airport.JFK,
                TotalPlaces = 1000,
                AvailablePlaces = 1000,
                Price = 1000
            } );
            flights.Add( new Flight
            {
                FlightId = "AF9545",
                From = Airport.CDG,
                To = Airport.DTW,
                TotalPlaces = 700,
                AvailablePlaces = 700,
                Price = 700
            } );
            flights.Add( new Flight
            {
                FlightId = "JF1296",
                From = Airport.JFK,
                To = Airport.CDG,
                TotalPlaces = 1000,
                AvailablePlaces = 1000,
                Price = 1000
            } );
            flights.Add( new Flight
            {
                FlightId = "JF1784",
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
