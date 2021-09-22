using FlyCie.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlyCie.App.Services
{
    public static class FlightsData
    {
        private static IEnumerable<Flight> _flights = new List<Flight>();
        public static IEnumerable<Flight> FlightList => _flights;

        public static IEnumerable<Flight> GetAvailableFlights()
        {
            return FlightList.Where( f => f.AvailablePlaces > 0 );
        }

        public static void SetFlights( IEnumerable<Flight> flights )
        {
            _flights = flights;
        }

        public static Flight GetFlight( string flightId )
        {
            return FlightList.FirstOrDefault( f => string.Equals( f.FlightId, flightId ) );
        }

        public static bool HasAvailablePlace( string flightId )
        {
            return GetFlight( flightId ).AvailablePlaces > 0;
        }

        public static Dictionary<string, List<Flight>> GetRoundTrips( List<string> flightIds )
        {
            var result = new Dictionary<string, List<Flight>>();
            result[ "RoundTrips" ] = new List<Flight>();
            result[ "OneWayTrips" ] = new List<Flight>();

            while( flightIds.Count > 0 )
            {
                var flight = GetFlight( flightIds[ 0 ] );
                var roundTrip = FlightList.First( f => f.From == flight.To && f.To == flight.From );

                if ( flightIds.Contains( roundTrip.FlightId ) )
                {
                    result[ "RoundTrips" ].Add( flight );
                    result[ "RoundTrips" ].Add( roundTrip );

                    flightIds.Remove( roundTrip.FlightId );
                }
                else
                {
                    result[ "OneWayTrips" ].Add( flight );
                }

                flightIds.Remove( flightIds[ 0 ] );
            }

            return result;
        }
    }
}
