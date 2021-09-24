using FlyCie.Model.External;
using System.Collections.Generic;

namespace FlyCie.Model.MTD
{
    public static class ModelMapper
    {
        public static FlightApi MapToFlightApi( MTDFlight flight )
        {
            return new FlightApi
            {
                AvailablePlaces = flight.AvailableSeats,
                FlightCode = flight.IdFlight,
                From = flight.DeparturePlace,
                Price = flight.BasePrice,
                To = flight.ArrivalPlace,
                Options = new List<FlightOptions>
                {
                    new FlightOptions
                    {
                        option_type = "AdditionalLuggage",
                        price = 100
                    }
                }
            };
        }
    }
}
