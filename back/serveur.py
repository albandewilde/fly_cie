#!/usr/bin/env python3

import bottle

srv = bottle.Bottle()


@srv.get("/")
def root():
    return "fly cie, how may I help you ?"


srv.run(host="0.0.0.0", port="7860")
