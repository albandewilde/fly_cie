#!/usr/bin/env python3

import json

import bottle

srv = bottle.Bottle()

flights = [
    {"id": 1, "from": "DTW", "to": "CDG"},
    {"id": 2, "from": "DTW", "to": "JFK"},
    {"id": 3, "from": "CDG", "to": "JFK"},
    {"id": 4, "from": "CDG", "to": "DTW"},
    {"id": 5, "from": "JFK", "to": "CDG"},
    {"id": 6, "from": "JFK", "to": "DTW"},
]


@srv.get("/")
def root():
    return "fly cie, how may I help you ?"


@srv.get("/flights")
def get_flights():
    return {"flights": flights}


srv.run(host="0.0.0.0", port="7860")
