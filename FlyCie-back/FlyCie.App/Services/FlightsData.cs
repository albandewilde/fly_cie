using FlyCie.App.Helpers;
using FlyCie.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FlyCie.App.Services
{
    public static class FlightsData
    {
        private static IEnumerable<Flight> _flights = new List<Flight>();
        private static IEnumerable<FlightApi> _externalFlights = new List<FlightApi>();
        public static IEnumerable<Flight> FlightList => _flights;
        public static IEnumerable<FlightApi> ExternalFlights => _externalFlights;

        public static IEnumerable<FlightApi> GetAvailableFlights()
        {
            var result = new List<FlightApi>();
            result.AddRange( FlightList.Where( f => f.AvailablePlaces > 0 ).Select( f => ExternalModelHelper.FlightToApi( f ) ) );
            result.AddRange( ExternalFlights.Where( f => f.AvailablePlaces > 0 ) );
            return result;
        }

        public static void SetFlights( 
            List<Flight> flights, 
            List<FlightApi> externalFlights,
            List<FlightApi> mtdFlights )
        {
            _flights = flights;
            _externalFlights = externalFlights;
            _externalFlights.ToList().AddRange( mtdFlights );
        }

        public static Flight GetFlight( string flightId, bool isOurs )
        {
            if( isOurs )
            {
                return FlightList.FirstOrDefault( f => string.Equals( f.FlightCode, flightId ) );
            }
            return ExternalFlights.FirstOrDefault( f => string.Equals( f.FlightCode, flightId ) );
        }

        public static bool HasAvailablePlace( string flightId, DateTime selectedDate )
        {
            var flight = GetFlight( flightId, true );
            var tickets = TicketsData.TicketList;

            var bookedPlaces = tickets.Where( t => t.Flight == flight && t.Date == selectedDate ).Count();
            return bookedPlaces < flight.TotalPlaces;
        }

        public static Dictionary<string, List<Flight>> GetRoundTrips( List<string> flightIds )
        {
            var result = new Dictionary<string, List<Flight>>();
            result[ "RoundTrips" ] = new List<Flight>();
            result[ "OneWayTrips" ] = new List<Flight>();

            while( flightIds.Count > 0 )
            {
                var flight = GetFlight( flightIds[ 0 ], true );
                var roundTrip = FlightList.FirstOrDefault( f => f.From == flight.To && f.To == flight.From );

                if ( !(roundTrip is null) && flightIds.Contains( roundTrip.FlightCode ) )
                {
                    result[ "RoundTrips" ].Add( flight );
                    result[ "RoundTrips" ].Add( roundTrip );

                    flightIds.Remove( roundTrip.FlightCode );
                }
                else
                {
                    result[ "OneWayTrips" ].Add( flight );
                }

                flightIds.Remove( flightIds[ 0 ] );
            }

            return result;
        }

        public static bool IsOurTrip( string flightCode )
        {
            if( FlightList.Where( f => f.FlightCode == flightCode ).Count() > 0 )
            {
                return true;
            }
            return false;
        }
    }
}
