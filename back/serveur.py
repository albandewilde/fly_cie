#!/usr/bin/env python3

# A faire lundi
# - Mettre le comportement des "controlleurs" dans des fonctions
# - Faire des TU parce que sinon c'est inmaintenable d'utiliser insomnia

import json

import bottle

from flights import flights, has_available_place, book_tickets
from tickets import tickets

srv = bottle.Bottle()


@srv.get("/")
def root():
    bottle.response.set_header("Access-Control-Allow-Origin", "*")
    return "Fly cie this is Pam, how may I help you ?"


@srv.post("/book")
def book_ticket():
    bottle.response.set_header("Access-Control-Allow-Origin", "*")
    body = json.loads(bottle.request.body.read().decode())
    client_tickets = book_tickets(
        body["first_name"],
        body["last_name"],
        body["nationality"],
        body["flight_ids"],
        tickets,
        flights,
    )
    jsn_tickets = json.dumps(client_tickets)

    return bottle.HTTPResponse(status=200, body=jsn_tickets)


@srv.get("/flights")
def get_flights():
    bottle.response.set_header("Access-Control-Allow-Origin", "*")
    return {"flights": flights["list"]}


srv.run(host="0.0.0.0", port="7860", debug=True)
