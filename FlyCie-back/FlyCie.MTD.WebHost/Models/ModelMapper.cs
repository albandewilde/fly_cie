using FlyCie.Model;
using FlyCie.Model.External;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlyCie.MTD.WebHost.Models
{
    public static class ModelMapper
    {
        public static FlightApi MapToFlightApi( MTDFlight flight )
        {
            var fields = new Dictionary<string, object>();
            fields.Add( "AdditionalLuggage", 0 );

            return new FlightApi
            {
                AvailablePlaces = flight.AvailableSeats,
                FlightCode = flight.IdFlight,
                From = flight.DeparturePlace,
                Price = flight.BasePrice,
                To = flight.ArrivalPlace,
                Options = null,
                AdditionalFields = fields
            };
        }
    }
}
