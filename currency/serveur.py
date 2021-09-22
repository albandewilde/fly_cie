#!/usr/bin/env python3

import json
import time

import bottle
import requests
from xml.etree import ElementTree
import xmltodict, json

srv = bottle.Bottle()

currencies = {}


def outdated(currency):
    return currency["date"] + (60 * 60 * 12) < time.time()


@srv.get("/")
def root():
    return "Currency thing is curently working"


@srv.get("/<currency>")
def get_currency_rate(currency):
    # Check if we have the currency in cache
    if currency in currencies and not outdated(currencies[currency]):
        return currencies[currency]["rate"]

    # Fetching the info
    try:
        response = requests.get(
            "https://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml"
        )
    except Exception:
        return bottle.HTTPResponse(status=503, body="Unable to get rate")

    tree = ElementTree.fromstring(response.content)
    all_currencies = xmltodict.parse(response.content)["gesmes:Envelope"]["Cube"][
        "Cube"
    ]["Cube"]

    rate = 1
    for c in all_currencies:
        if c["@currency"] == currency:
            rate = c["@rate"]

            # Add info in our cache
            currencies[c["@currency"]] = {"rate": rate, "date": time.time()}

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
    bottle.response.set_header("Access-Control-Allow-Origin", "*")
    return json.dumps(currencies)


srv.run(host="0.0.0.0", port="7861", debug=True)
