import datetime
import requests
from xml.etree import ElementTree
import xmltodict, json

tickets = []


def create_ticket(user_last_name, user_first_name, nationality, flight_id, price, lounge_supplement, currency):
    if lounge_supplement:
        price += 150
    
    rate = get_currency_rate( currency )

    return {
        "last_name": user_last_name,
        "first_name": user_first_name,
        "nationality": nationality,
        "flight_id": int(flight_id),
        "price": price,
        "creation_date": str(datetime.datetime.now()),
        "lounge_supplement": lounge_supplement,
        "currency": currency,
        "rate": rate
    }

def get_converted_price( currency, rate, price ):
    return price * rate

def get_currency_rate( currency ):
    response = requests.get("https://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml")
    tree = ElementTree.fromstring(response.content)
    all_currencies = xmltodict.parse(response.content)["gesmes:Envelope"]["Cube"]["Cube"]["Cube"]
    
    rate = 1
    for c in all_currencies:
        if c["@currency"] == currency:
            rate = c["@rate"]
    
    return rate
