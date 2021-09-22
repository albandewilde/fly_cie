using FlyCie.Model;

namespace FlyCie.External.WebHost.Helpers
{
    public static class ExternalModelHelper
    {
        public static Flight MapFlight( Model.External.Flight flight )
        {
            return new Flight
            {
                AvailablePlaces = flight.plane.total_seats - flight.seats_booked,
                FlightId = flight.code,
                From = flight.departure,
                To = flight.arrival,
                Price = flight.base_price,
                TotalPlaces = flight.plane.total_seats
            };
        }
    }
}
