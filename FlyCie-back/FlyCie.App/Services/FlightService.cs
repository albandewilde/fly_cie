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

            flights.Add( new Flight
            {
                flightCode = "DT3712",
                from = Airport.DTW.ToString(),
                to = Airport.CDG.ToString(),
                totalPlaces = 700,
                availablePlaces = 700,
                price = 700
            } );
            flights.Add( new Flight
            {
                flightCode = "DT3333",
                from = Airport.DTW.ToString(),
                to = Airport.JFK.ToString(),
                totalPlaces = 300,
                availablePlaces = 300,
                price = 300
            } );
            flights.Add( new Flight
            {
                flightCode = "AF2458",
                from = Airport.CDG.ToString(),
                to = Airport.JFK.ToString(),
                totalPlaces = 1000,
                availablePlaces = 1000,
                price = 1000
            } );
            flights.Add( new Flight
            {
                flightCode = "AF9545",
                from = Airport.CDG.ToString(),
                to = Airport.DTW.ToString(),
                totalPlaces = 700,
                availablePlaces = 700,
                price = 700
            } );
            flights.Add( new Flight
            {
                flightCode = "JF1296",
                from = Airport.JFK.ToString(),
                to = Airport.CDG.ToString(),
                totalPlaces = 1000,
                availablePlaces = 1000,
                price = 1000
            } );
            flights.Add( new Flight
            {
                flightCode = "JF1784",
                from = Airport.JFK.ToString(),
                to = Airport.DTW.ToString(),
                totalPlaces = 300,
                availablePlaces = 300,
                price = 300
            } );

            _logger.LogInformation( "Fetching external flights." );
            var externalFlights = await _externalService.GetExternalFlights();
            var flightsToRemove = flights.FindAll( f => externalFlights.ToList().FindIndex( ef => ef.from == f.from && ef.to == f.to ) >= 0 );
            flights.RemoveAll( f => flightsToRemove.Contains( f ) );

            _logger.LogInformation( "Fetching MTD Flights" );
            var mtdFlights = await _externalService.GetMTDFlights();
            FlightsData.SetFlights( flights, externalFlights.ToList(), mtdFlights.ToList() );
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
