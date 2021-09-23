using FlyCie.Model;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FlyCie.App.Services
{
    public class FlightService : BackgroundService
    {
        private readonly ILogger<FlightService> _logger;
        private readonly ExternalService _externalService;

        public FlightService( 
            ILogger<FlightService> logger,
            ExternalService externalService )
        {
            _logger = logger;
            _externalService = externalService;
        }
        
        public async Task InitializeData()
        {
            var flights = new List<Flight>();
            var allFlights = new Dictionary<string, List<Flight>>();

            flights.Add( new Flight
            {
                FlightCode = "DT3712",
                From = Airport.DTW,
                To = Airport.CDG,
                TotalPlaces = 700,
                AvailablePlaces = 700,
                Price = 700
            } );
            flights.Add( new Flight
            {
                FlightCode = "DT3333",
                From = Airport.DTW,
                To = Airport.JFK,
                TotalPlaces = 300,
                AvailablePlaces = 300,
                Price = 300
            } );
            flights.Add( new Flight
            {
                FlightCode = "AF2458",
                From = Airport.CDG,
                To = Airport.JFK,
                TotalPlaces = 1000,
                AvailablePlaces = 1000,
                Price = 1000
            } );
            flights.Add( new Flight
            {
                FlightCode = "AF9545",
                From = Airport.CDG,
                To = Airport.DTW,
                TotalPlaces = 700,
                AvailablePlaces = 700,
                Price = 700
            } );
            flights.Add( new Flight
            {
                FlightCode = "JF1296",
                From = Airport.JFK,
                To = Airport.CDG,
                TotalPlaces = 1000,
                AvailablePlaces = 1000,
                Price = 1000
            } );
            flights.Add( new Flight
            {
                FlightCode = "JF1784",
                From = Airport.JFK,
                To = Airport.DTW,
                TotalPlaces = 300,
                AvailablePlaces = 300,
                Price = 300
            } );
            allFlights[ "Flights" ] = flights;

            _logger.LogInformation( "Fetching external flights." );
            var externalFlights = await _externalService.GetExternalFlights();
            var flightsToRemove = allFlights[ "Flights" ].FindAll( f => externalFlights.ToList().FindIndex( ef => ef.From == f.From && ef.To == f.To ) >= 0 );
            allFlights[ "Flights" ].RemoveAll( f => flightsToRemove.Contains( f ) );

            if( !( externalFlights is null ) )
            {
                allFlights[ "ExternalFlights" ] = externalFlights.ToList();
            }

            FlightsData.SetFlights( allFlights );
        }

        protected override async Task ExecuteAsync( CancellationToken stoppingToken )
        {
            while ( true )
            {
                _logger.LogInformation( "Initializing possible flights." );
                await InitializeData();
                _logger.LogInformation( "Flights initialized." );
                await Task.Delay( 1000 * 60 * 60 * 6 );
            }
        }
    }
}
