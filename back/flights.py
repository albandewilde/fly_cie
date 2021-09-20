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

def book_ticket(user_last_name, user_first_name, nationality, flight_id):
    ticket = {
        "last_name": user_last_name,
        "first_name": user_first_name,
        "nationality": nationality,
        "flight_id": int(flight_id),
        "price": get_flight(int(flight_id), flights["list"])["price"],
        "creation_date": str(datetime.datetime.now())
    }
    tickets.append(ticket)

    get_flight(flight_id, flights["list"])["available_places"] -= 1

    return ticket