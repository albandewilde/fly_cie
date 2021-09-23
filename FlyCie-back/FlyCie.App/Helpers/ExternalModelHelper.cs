using FlyCie.Model;
using System;
using System.Collections.Generic;

namespace FlyCie.App.Helpers
{
    public static class ExternalModelHelper
    {
        public static Flight MapFlight( Model.External.Flight flight )
        {
            return new Flight
            {
                AvailablePlaces = flight.plane.total_seats - flight.seats_booked,
                FlightCode = flight.code,
                From = Enum.Parse<Airport>( flight.departure ),
                To = Enum.Parse<Airport>( flight.arrival ),
                Price = flight.base_price,
                TotalPlaces = flight.plane.total_seats
            };
        }
        
        public static Model.External.Ticket MapTicket( Ticket ticket )
        {
            return new Model.External.Ticket
            {
                code = ticket.Flight.FlightCode,
                customer_name = ticket.LastName,
                customer_nationality = ticket.Nationality,
                flight = new Model.External.Flight
                {
                    arrival = ticket.Flight.To.ToString(),
                    code = ticket.Flight.FlightCode,
                    base_price = Convert.ToInt32( ticket.Price ),
                    departure = ticket.Flight.From.ToString(),
                    plane = new Model.External.Plane
                    {
                        name = string.Empty,
                        total_seats = ticket.Flight.TotalPlaces
                    },
                    seats_booked = ticket.Flight.TotalPlaces - ticket.Flight.AvailablePlaces
                },
                date = DateTime.UtcNow.ToString("dd-MM-yyyy"),
                options = new List<Model.External.FlightOptions>(),
                booking_source = "BAA",
                payed_price = Convert.ToInt32( ticket.Price * ticket.Currency.Rate )
            };
        }
    }
}
