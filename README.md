# fly_cie
School project for architecture course

# Endpoints
GET :  
/flights   
=> returns an Array of Flight
{
    "id": 1,
    "from": "DTW",
    "to": "CDG"
}

POST :  
/book   
=> body contains { lastName, firstName, nationality, flightId }
=> returns the created ticket