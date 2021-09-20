from multiprocessing import Lock
from tickets import tickets, create_ticket

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


def get_round_trips(flight_ids, flights):
    round_trips = []
    trips = []

    for id in flight_ids:
        flight = get_flight(id, flights)
        for end_id in flight_ids[id + 1:]:
            second_flight = get_flight(end_id, flights)
            if (
                flight["from"] == second_flight["to"]
                and flight["to"] == second_flight["from"]
            ):
                round_trips.append((flight, second_flight))
                flight_ids.remove(end_id)

                break
        else:
            trips.append(flight)

    print(round_trips, trips)
    return (round_trips, trips)


def book_tickets(fname, lname, nat, flight_ids, lounge_supplement, currency, tickets, flights):
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
        round_trip_tickets = book_round_trip(
            round_trip, flights["list"], lname, fname, nat, lounge_supplement, currency
        )
        new_tickets.extend(round_trip_tickets)
        tickets.extend(round_trip_tickets)

    # Book tickets
    for trip in trips:
        ticket = book_trip(trip, flights["list"], lname, fname, nat, lounge_supplement, currency)
        new_tickets.append(ticket)
        tickets.append(ticket)

    # Free the lock
    flights["mux"].release()

    return new_tickets


def book_round_trip(round_trip, flights, lname, fname, nat, lounge_supplement, currency):
    result = []

    f = get_flight(round_trip[0]["id"], flights)
    first_ticket = create_ticket(
        lname, fname, nat, f["id"], f["price"] * 0.9, lounge_supplement, currency
    )
    f["available_places"] -= 1
    result.append(first_ticket)

    second_f = get_flight(round_trip[0]["id"], flights)
    second_ticket = create_ticket(
        lname, fname, nat, second_f["id"], second_f["price"] * 0.9, lounge_supplement, currency
    )
    second_f["available_places"] -= 1
    result.append(second_ticket)

    return result


def book_trip(trip, flights, lname, fname, nat, lounge_supplement):
    flight = get_flight(trip["id"], flights)
    ticket = create_ticket(lname, fname, nat, flight["id"], flight["price"], lounge_supplement, currency)
    flight["available_places"] -= 1

    return ticket
