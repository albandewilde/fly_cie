from multiprocessing import Lock
import datetime
from tickets import tickets

flights = {
    "list": [
        {
            "id": 1,
            "from": "DTW",
            "to": "CDG",
            "available_places": 700,
            "total_places": 700,
            "price": 700,
        },
        {
            "id": 2,
            "from": "DTW",
            "to": "JFK",
            "available_places": 300,
            "total_places": 300,
            "price": 300,
        },
        {
            "id": 3,
            "from": "CDG",
            "to": "JFK",
            "available_places": 1000,
            "total_places": 1000,
            "price": 1000,
        },
        {
            "id": 4,
            "from": "CDG",
            "to": "DTW",
            "available_places": 700,
            "total_places": 700,
            "price": 700,
        },
        {
            "id": 5,
            "from": "JFK",
            "to": "CDG",
            "available_places": 1000,
            "total_places": 1000,
            "price": 1000,
        },
        {
            "id": 6,
            "from": "JFK",
            "to": "DTW",
            "available_places": 300,
            "total_places": 300,
            "price": 300,
        },
    ],
    "mux": Lock(),
}


def get_flight(flight_id, flights):
    for flight in flights:
        if flight["id"] == flight_id:
            return flight


def has_available_place(flight_id, flights):
    for flight in flights:
        if flight["id"] == flight_id:
            if flight["available_places"] > 0:
                return True
            else:
                return False


def book_tickets(fname, lname, nat, flight_ids, tickets, flights):
    # Lock flights
    flights["mux"].acquire()

    # Check if there is place
    for fly_id in flight_ids:
        if not has_available_place(fly_id, flights["list"]):
            flights["mux"].release()
            raise Exception(f"Flight {fly_id} has no available place")

    new_tickets = []

    # Get round trip
    round_trips, trips = get_round_trips(flight_ids, flights["list"])

    # Book tickets
    # Book round trips
    for round_trip in round_trips:
        round_trip_tickets = book_round_trip(round_trip, flights["list"], lname, fname, nat)
        new_tickets.extend(round_trip_tickets)
        tickets.extends(round_trip_tickets)
    # Book tickets
    for trip in trips:
        ticket = book_trip(trip, flights["list"], lname, fname, nat)
        new_tickets.append(ticket)
        tickets.append(ticket)

    # Free the lock
    flights["mux"].release()

    return new_tickets


def get_round_trips(flight_ids, flights):
    round_trips = []
    trips = []

    for id in flight_ids:
        flight = get_flight(id, flights)
        for end_id in flight_ids[id:]:
            second_flight = get_flight(id, flights)
            if (
                flight["from"] == second_flight["to"]
                and flight["to"] == second_flight["from"]
            ):
                round_trips.append((flight, plane))
                break
        else:
            trips.append(flight)

    return (round_trips, trips)


def book_round_trip(round_trip, flights, lname, fname, nat):
    result = []

    f = get_flight(round_trip[0]["flight_id"])
    first_ticket = create_ticket(
        lname, fname, nat, f["flight_id"], f["price"] * 0.9
    )
    f["available_places"] -= 1
    result.append(first_ticket)

    second_f = get_flight(round_trip[0]["flight_id"])
    second_ticket = create_ticket(
        lname, fname, nat, second_f["flight_id"], second_f["price"] * 0.9
    )
    second_f["available_places"] -= 1
    result.append(second_f)

    return result

def book_trip(trip, flights, lname, fname, nat):
    print(trip)
    flight = get_flight(trip["flight_id"], flights)
    ticket = create_ticket(
        lname, fname, nat, flight["flight_id"], flight["price"]
    )
    flight["available_places"] -= 1
    
    return ticket