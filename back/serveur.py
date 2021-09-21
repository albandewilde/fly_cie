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

# the decorator
def enable_cors(fn):
    def _enable_cors(*args, **kwargs):
        # set CORS headers
        bottle.response.headers['Access-Control-Allow-Origin'] = '*'
        bottle.response.headers['Access-Control-Allow-Methods'] = 'GET, POST, PUT, OPTIONS'
        bottle.response.headers['Access-Control-Allow-Headers'] = 'Origin, Accept, Content-Type, X-Requested-With, X-CSRF-Token'

        if bottle.request.method != 'OPTIONS':
            # actual request; reply with the actual response
            return fn(*args, **kwargs)

    return _enable_cors

@srv.post("/book")
@enable_cors
def book_ticket():
    body = json.loads(bottle.request.body.read().decode())
    client_tickets = book_tickets(
        body["first_name"],
        body["last_name"],
        body["nationality"],
        body["flight_ids"],
        body["lounge_supplement"],
        body["currency"],
        tickets,
        flights,
    )
    jsn_tickets = json.dumps(client_tickets)

    return bottle.HTTPResponse(status=200, body=jsn_tickets)


@srv.get("/flights")
@enable_cors
def get_flights():
    return {"flights": flights["list"]}

srv.run(host="0.0.0.0", port="7860", debug=True)
