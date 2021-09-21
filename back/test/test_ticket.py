import pytest

from tickets import create_ticket


def test_create_ticket():
    ticket = create_ticket("lname", "fname", "nat", 2, 400, False)

    assert "lname" == ticket["last_name"]
    assert "fname" == ticket["first_name"]
    assert "nat" == ticket["nationality"]
    assert 2 == ticket["flight_id"]
    assert 400 == ticket["price"]
    assert not ticket["lounge_supplement"]


def test_create_ticket_with_lounge():
    ticket = create_ticket("lname", "fname", "nat", 2, 400, True)

    assert "lname" == ticket["last_name"]
    assert "fname" == ticket["first_name"]
    assert "nat" == ticket["nationality"]
    assert 2 == ticket["flight_id"]
    assert 550 == ticket["price"]
    assert ticket["lounge_supplement"]
