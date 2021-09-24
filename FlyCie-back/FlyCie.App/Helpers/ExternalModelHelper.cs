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
                availablePlaces = flight.plane.total_seats - flight.seats_booked,
                flightCode = flight.code,
                from = flight.departure,
                to = flight.arrival,
                price = flight.base_price,
                totalPlaces = flight.plane.total_seats
            };
        }

        public static FlightApi MapFlightApi( Model.External.Flight flight )
        {
            return new FlightApi
            {
                availablePlaces = flight.plane.total_seats - flight.seats_booked,
                flightCode = flight.code,
                from = flight.departure,
                to = flight.arrival,
                price = flight.base_price,
                totalPlaces = flight.plane.total_seats,
                Options = flight.options,
                AdditionalFields = null,
                BookingSource = "External"
            };
        }

        public static FlightApi FlightToApi( Flight flight )
        {
            return new FlightApi
            {
                availablePlaces = flight.availablePlaces,
                flightCode = flight.flightCode,
                from = flight.from,
                to = flight.to,
                price = flight.price,
                totalPlaces = flight.totalPlaces,
                Options = null,
                AdditionalFields = null
            };
        }
        
        public static Model.External.Ticket MapTicket( Ticket ticket )
        {
            return new Model.External.Ticket
            {
                code = ticket.Flight.flightCode,
                customer_name = ticket.LastName,
                customer_nationality = ticket.Nationality,
                flight = new Model.External.Flight
                {
                    arrival = ticket.Flight.to.ToString(),
                    code = ticket.Flight.flightCode,
                    base_price = Convert.ToInt32( ticket.Price ),
                    departure = ticket.Flight.from.ToString(),
                    plane = new Model.External.Plane
                    {
                        name = string.Empty,
                        total_seats = ticket.Flight.totalPlaces
                    },
                    seats_booked = ticket.Flight.totalPlaces - ticket.Flight.availablePlaces
                },
                date = ticket.Date.ToString("dd-MM-YYYY"),
                options = new List<Model.External.FlightOptions>(),
                booking_source = "BAA",
                payed_price = Convert.ToInt32( ticket.Price * ticket.Currency.Rate )
            };
        }

        public static Ticket MapTicket( Model.External.Ticket ticket )
        {
            return new Ticket
            {
                Date = DateTime.Parse( ticket.date ),
                FirstName = ticket.customer_name,
                Flight = MapFlight( ticket.flight ),
                LastName = ticket.customer_name,
                Nationality = ticket.customer_nationality,
                Price = ticket.payed_price,
                LoungeSupplement = false
            };
        }
    }
}
