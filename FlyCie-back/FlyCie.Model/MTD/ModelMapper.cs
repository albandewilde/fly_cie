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
                availablePlaces = flight.availableSeats,
                flightCode = flight.idFlight,
                from = flight.departurePlace,
                price = flight.basePrice,
                to = flight.arrivalPlace,
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
