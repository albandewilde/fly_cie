from multiprocessing import Lock
import pytest

from flights import (
    flights,
    get_flight,
    has_available_place,
    book_tickets,
    get_round_trips,
)

from tickets import create_ticket


def test_get_flight_from_id():
    flgt = get_flight(
        6,
        [
            {
                "id": 6,
                "from": "JFK",
                "to": "DTW",
                "available_places": 300,
                "total_places": 300,
                "price": 300,
            },
        ],
    )

    assert flgt["id"] == 6
    assert flgt["from"] == "JFK"
    assert flgt["to"] == "DTW"
    assert flgt["available_places"] == 300
    assert flgt["total_places"] == 300
    assert flgt["price"] == 300


def test_get_non_existing_flight_return_none():
    flgt = get_flight(
        -12,
        [
            {
                "id": 6,
                "from": "JFK",
                "to": "DTW",
                "available_places": 300,
                "total_places": 300,
                "price": 300,
            },
        ],
    )

    assert flgt == None


def test_available_flight():
    assert has_available_place(
        1,
        [
            {
                "id": 1,
                "from": "JFK",
                "to": "DTW",
                "available_places": 300,
                "total_places": 300,
                "price": 300,
            },
        ],
    )


def test_get_round_trips():
    round_trips, trips = get_round_trips(
        [1, 2, 4],
        [
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
        ],
    )

    assert 1 == len(trips)
    assert 2 == trips[0]["id"]
    assert 1 == len(round_trips)
    assert 1 == round_trips[0][0]["id"]
    assert 4 == round_trips[0][1]["id"]


def test_get_round_trips_with_two_come_back():
    round_trips, trips = get_round_trips(
        [1, 4, 4],
        [
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
        ],
    )

    assert 1 == len(trips)
    assert 4 == trips[0]["id"]
    assert 1 == len(round_trips)
    assert 1 == round_trips[0][0]["id"]
    assert 4 == round_trips[0][1]["id"]


def test_get_round_trips_with_two_same_round_trip():
    round_trips, trips = get_round_trips(
        [1, 4, 4, 1],
        [
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
        ],
    )

    assert 0 == len(trips)
    assert 2 == len(round_trips)
    assert 1 == round_trips[0][0]["id"]
    assert 4 == round_trips[0][1]["id"]
    assert 4 == round_trips[1][0]["id"]
    assert 1 == round_trips[1][1]["id"]


def test_get_round_trips_with_two_round_trip():
    round_trips, trips = get_round_trips(
        [1, 3, 4, 5],
        [
            {
                "id": 1,
                "from": "DTW",
                "to": "CDG",
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
        ],
    )

    assert 0 == len(trips)
    assert 2 == len(round_trips)
    assert 1 == round_trips[0][0]["id"]
    assert 4 == round_trips[0][1]["id"]
    assert 3 == round_trips[1][0]["id"]
    assert 5 == round_trips[1][1]["id"]


def test_booking_ticket():
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
                "id": 5,
                "from": "JFK",
                "to": "CDG",
                "available_places": 1000,
                "total_places": 1000,
                "price": 1000,
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
        ],
        "mux": Lock(),
    }

    all_tickets = []
    tickets = book_tickets(
        "fname", "lname", "FR", [1, 4, 3], False, "", all_tickets, flights
    )

    assert tickets == all_tickets
    assert 3 == len(tickets)
    for idx in range(3):
        assert "lname" == tickets[idx]["last_name"]
        assert "fname" == tickets[idx]["first_name"]
        assert "FR" == tickets[idx]["nationality"]
        assert not tickets[idx]["lounge_supplement"]

    print(tickets)
    assert 1 == tickets[0]["flight_id"]
    assert 4 == tickets[1]["flight_id"]
    assert 3 == tickets[2]["flight_id"]
    assert 630 == tickets[0]["price"] == tickets[1]["price"]
    assert 1000 == tickets[2]["price"]
