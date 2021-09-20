from multiprocessing import Lock
import pytest

from flights import flights, get_flight, has_available_place, book_ticket


def test_get_flight_from_id():
    flgt = get_flight(6, flights["list"])

    assert flgt["id"] == 6
    assert flgt["from"] == "JFK"
    assert flgt["to"] == "DTW"
    assert flgt["available_places"] == 300
    assert flgt["total_places"] == 300
    assert flgt["price"] == 300


def test_get_non_existing_flight_return_none():
    flgt = get_flight(-12, flights["list"])

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


def test_booking_ticket():
    flights = [
        {
            "id": 2,
            "from": "JFK",
            "to": "DTW",
            "available_places": 300,
            "total_places": 300,
            "price": 300,
        },
    ]
    ticket = book_ticket("lname", "fname", "FR", 2, [], flights)

    assert 299 == flights[0]["available_places"]
    assert "lname" == ticket["last_name"]
    assert "fname" == ticket["first_name"]
    assert "FR" == ticket["nationality"]
    assert 2 == ticket["flight_id"]
    assert 300 == ticket["price"]
