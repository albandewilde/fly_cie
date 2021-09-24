using FlyCie.Model;
using FlyCie.Model.External;
using FlyCie.Model.MTD;
using System.Collections.Generic;

namespace FlyCie.App.Helpers
{
    public static class MTDModelMapper
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
                },
                BookingSource = "MTD"
            };
        }
    }
}
