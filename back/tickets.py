tickets = []

def create_ticket(user_last_name, user_first_name, nationality, flight_id, price):
    return {
        "last_name": user_last_name,
        "first_name": user_first_name,
        "nationality": nationality,
        "flight_id": int(flight_id),
        "price": price,
        "creation_date": str(datetime.datetime.now()),
    }