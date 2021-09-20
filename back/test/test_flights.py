from multiprocessing import Lock
import pytest

from flights import (
    flights,
    get_flight,
    has_available_place,
    book_tickets,
    get_round_trips,
)


# def test_get_flight_from_id():
#    flgt = get_flight(6, flights["list"])
#
#    assert flgt["id"] == 6
#    assert flgt["from"] == "JFK"
#    assert flgt["to"] == "DTW"
#    assert flgt["available_places"] == 300
#    assert flgt["total_places"] == 300
#    assert flgt["price"] == 300
#
#
# def test_get_non_existing_flight_return_none():
#    flgt = get_flight(-12, flights["list"])
#
#    assert flgt == None
#
#
# def test_available_flight():
#    assert has_available_place(
#        1,
#        [
#            {
#                "id": 1,
#                "from": "JFK",
#                "to": "DTW",
#                "available_places": 300,
#                "total_places": 300,
#                "price": 300,
#            },
#        ],
#    )
#
#
# def test_booking_ticket():
#    flights = [
#        {
#            "id": 2,
#            "from": "JFK",
#            "to": "DTW",
#            "available_places": 300,
#            "total_places": 300,
#            "price": 300,
#        },
#    ]
#    ticket = book_ticket("lname", "fname", "FR", 2, [], flights)
#
#    assert 299 == flights[0]["available_places"]
#    assert "lname" == ticket["last_name"]
#    assert "fname" == ticket["first_name"]
#    assert "FR" == ticket["nationality"]
#    assert 2 == ticket["flight_id"]
#    assert 300 == ticket["price"]


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
    assert 2 == trips[0]["id"]
    assert 1 == round_trips[0][0]["id"]
    assert 4 == round_trips[0][1]["id"]
