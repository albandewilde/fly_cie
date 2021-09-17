#!/usr/bin/env python3

# A faire lundi
# - Ne pas envoyer toutes les info des flights
# - Ajouté une condition des booking afin de ne pas vendre plus de places qu'il
#   y en a (comme la SNCF) donc ajouté un mutex à la structure flights
# - Mettre le comportement des "controlleurs" dans des fonctions
# - Faire des TU parce que sinon c'est inmaintenable d'utiliser insomnia
# - Faire un package avec flights (et les TU pour la fonction get_flight

import json

import bottle

srv = bottle.Bottle()

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


def get_flight(flight_id):
    for flight in flights:
        if flight["id"] == flight_id:
            return flight


tickets = []


@srv.get("/")
def root():
    return "Fly cie this is Pam, how may I help you ?"


@srv.post("/book")
def create_ticket():
    body = json.loads(bottle.request.body.read().decode())

    ticket = {
        "last_name": body["last_name"],
        "first_name": body["first_name"],
        "nationality": body["nationality"],
        "flight_id": int(body["flight_id"]),
        "price": get_flight(int(body["flight_id"]))["price"],
    }

    tickets.append(ticket)

    return bottle.HTTPResponse(status=200, body=ticket)


@srv.get("/flights")
def get_flights():
    return {"flights": flights}


srv.run(host="0.0.0.0", port="7860")
