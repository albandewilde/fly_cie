# fly_cie
School project for architecture course

# Endpoints

The server run on ther port `7860` on all interfaces

GET :  
/flights   
=> returns an Array of Flight
```json
{
    "id": 1,
    "from": "DTW",
    "to": "CDG",
    "price": 700
}
```

POST :  
/book   
=> body contains { lastName, firstName, nationality, flightId }
=> returns the created ticket
