#!/usr/bin/env python3

# A faire lundi
# - Ne pas envoyer toutes les info des flights
# - Ajouté une condition des booking afin de ne pas vendre plus de places qu'il
#   y en a (comme la SNCF) donc ajouté un mutex à la structure flights
# - Mettre le comportement des "controlleurs" dans des fonctions
# - Faire des TU parce que sinon c'est inmaintenable d'utiliser insomnia
# - Faire un package avec flights (et les TU pour la fonction get_flight

import json
import datetime
from multiprocessing import Process, Lock

import bottle

from flights import flights, get_flight, has_available_place
from tickets import tickets

srv = bottle.Bottle()


@srv.get("/")
def root():
    return "Fly cie this is Pam, how may I help you ?"


@srv.post("/book")
def create_ticket():
    flights["mux"].acquire()
    body = json.loads(bottle.request.body.read().decode())
    flight_id = body["flight_id"]

    if not has_available_place(flight_id):
        flights["mux"].release()
        return bottle.HTTPResponse(status=400, body="No available place sorry bro.")

    ticket = {
        "last_name": body["last_name"],
        "first_name": body["first_name"],
        "nationality": body["nationality"],
        "flight_id": int(body["flight_id"]),
        "price": get_flight(int(flight_id), flights)["price"],
        "creation_date": datetime.datetime.now()
    }
    tickets.append(ticket)

    get_flight(flight_id)["available_places"] -= 1
    
    flights["mux"].release()
    return bottle.HTTPResponse(status=200, body=ticket)


@srv.get("/flights")
def get_flights():
    return {"flights": flights}


srv.run(host="0.0.0.0", port="7860")
