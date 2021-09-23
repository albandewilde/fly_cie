using FlyCie.Model;
using System.Collections.Generic;
using System.Linq;

namespace FlyCie.App.Services
{
    public static class FlightsData
    {
        private static IEnumerable<Flight> _flights = new List<Flight>();
        private static IEnumerable<Flight> _externalFlights = new List<Flight>();
        public static IEnumerable<Flight> FlightList => _flights;
        public static IEnumerable<Flight> ExternalFlights => _externalFlights;

        public static IEnumerable<Flight> GetAvailableFlights()
        {
            var result = new List<Flight>();
            result.AddRange( FlightList.Where( f => f.AvailablePlaces > 0 ) );
            result.AddRange( ExternalFlights.Where( f => f.AvailablePlaces > 0 ) );
            return result;
        }

        public static void SetFlights( Dictionary<string, List<Flight>> flights )
        {
            _flights = flights[ "Flights" ];
            _externalFlights = flights[ "ExternalFlights" ];
        }

        public static Flight GetFlight( string flightId, bool isOurs )
        {
            if( isOurs )
            {
                return FlightList.FirstOrDefault( f => string.Equals( f.FlightCode, flightId ) );
            }
            return ExternalFlights.FirstOrDefault( f => string.Equals( f.FlightCode, flightId ) );
        }

        public static bool HasAvailablePlace( string flightId )
        {
            return GetFlight( flightId, true ).AvailablePlaces > 0;
        }

        public static Dictionary<string, List<Flight>> GetRoundTrips( List<string> flightIds )
        {
            var result = new Dictionary<string, List<Flight>>();
            result[ "RoundTrips" ] = new List<Flight>();
            result[ "OneWayTrips" ] = new List<Flight>();

            while( flightIds.Count > 0 )
            {
                var flight = GetFlight( flightIds[ 0 ], true );
                var roundTrip = FlightList.First( f => f.From == flight.To && f.To == flight.From );

                if ( flightIds.Contains( roundTrip.FlightCode ) )
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
