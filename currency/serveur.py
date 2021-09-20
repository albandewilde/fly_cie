#!/usr/bin/env python3

import json

import bottle
import requests
from xml.etree import ElementTree
import xmltodict, json

srv = bottle.Bottle()


@srv.get("/")
def root():
    return "Currency thing is curently working"


@srv.get("/<currency>")
def get_currency_rate(currency):
    response = requests.get(
        "https://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml"
    )
    tree = ElementTree.fromstring(response.content)
    all_currencies = xmltodict.parse(response.content)["gesmes:Envelope"]["Cube"][
        "Cube"
    ]["Cube"]

    rate = 1
    for c in all_currencies:
        if c["@currency"] == currency:
            rate = c["@rate"]

    return rate


@srv.get("/currencies")
def get_currencies():
    response = requests.get(
        "https://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml"
    )
    tree = ElementTree.fromstring(response.content)
    all_currencies = xmltodict.parse(response.content)["gesmes:Envelope"]["Cube"][
        "Cube"
    ]["Cube"]

    currencies = [c["@currency"] for c in all_currencies]
    return json.dumps(currencies)


srv.run(host="0.0.0.0", port="7861")
