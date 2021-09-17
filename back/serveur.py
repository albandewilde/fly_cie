#!/usr/bin/env python3

import bottle

srv = bottle.Bottle()

tickets = []

@srv.get("/")
def root():
    return "fly cie, how may I help you ?"

@srv.post("/book")
def create_ticket( lastName, firstName, nationality, flightId ):
    ticket = {
        "LastName": lastName,
        "FirstName": firstName,
        "Nationality": nationality,
        "FlightId": flightId
    }
    tickets.append(ticket)
    return bottle.HTTPResponse(status=200, body=ticket)

srv.run(host="0.0.0.0", port="7860")
