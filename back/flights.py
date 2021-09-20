from multiprocessing import Lock

flights = [
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
]

flights = {
    "list": flights,
    "mux": Lock()
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
