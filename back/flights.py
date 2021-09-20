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


def book_ticket(
    user_last_name, user_first_name, nationality, flight_id, tickets, flights
):
    ticket = create_ticket()
    tickets.append(ticket)

    get_flight(flight_id, flights)["available_places"] -= 1

    return ticket

def get_round_trips( flight_ids, flights ):
    round_trips = []
    trips = []

    for id in flight_ids:
        flight = get_flight( id, flights )   
        for end_id in flight_ids[id:]:
            second_flight = get_flight( id, flights )
            if flight["from"] == second_flight["to"] and flight["to"] == second_flight["from"]:
                round_trips.append( (flight, plane) )
                break
        else:
            trips.append( flight )

    return (round_trips, trips)