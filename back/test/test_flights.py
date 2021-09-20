import pytest

from flights import flights, get_flight


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
