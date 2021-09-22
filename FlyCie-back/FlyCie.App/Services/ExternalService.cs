using FlyCie.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FlyCie.App.Services
{
    public class ExternalService
    {
        public ExternalService()
        {

        }

        public Dictionary<string, IEnumerable<Flight>> GroupFlights(
            IEnumerable<Flight> internalFlights,
            IEnumerable<Flight> externalFlights )
        {
            var result = new Dictionary<string, IEnumerable<Flight>>();
            var flightsToRemove = internalFlights.ToList().Where( f => ShouldRemove( f, externalFlights ) );
            internalFlights.ToList().RemoveAll( f => flightsToRemove.Contains( f ) );

            result[ "InternalFlights" ] = internalFlights;
            result[ "ExternalFlights" ] = externalFlights;

            return result;
        }

        private bool ShouldRemove( Flight internalFlight, IEnumerable<Flight> externalFlights )
        {
            if ( externalFlights.Contains( internalFlight ) )
            {
                return true;
            }
            return false;
        }
    }
}
