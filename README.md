# fly_cie

School project for architecture course

You can start the project with the command `docker-compose up`.  
It will start:

- The Front on port `7862`
- The Back on port `7860`
- The Currency on port `7861`
- The External on port `7863`
- The Out on port `7864`

# Backend endpoints

The server run on ther port `7860` on all interfaces

GET :  
api/flight/getFlights
=> returns an Array of Flight

enum Airport {
    DTW,
    CDG,
    JFK
}

```json
{
    "FlightId": 1,
    "From": 1,
    "To": 2,
    "AvailablePlaces": 700,
    "TotalPlaces": 700,
    "Price": 700
}
```

POST :  
api/flight/bookTicket
=> body contains
```json
{
    "LastName": "string",
    "FirstName": "string",
    "Nationality": "string",
    "FlightIds": [1, 2, 3],
    "LoungeSupplement": true,
    "Currency": "string"
}
```
=> returns the created tickets
```json
[
    {
        "LastName": "string",
        "FirstName": "string",
        "Nationality": "string",
        "Flight": "Flight",
        "Price": 300,
        "Date": "string",
        "LoungeSupplement": true,
        "Currency": {
            "name": "string",
            "rate": 0
        }
    },
    {
        "LastName": "string",
        "FirstName": "string",
        "Nationality": "string",
        "Flight": "Flight",
        "Price": 300,
        "Date": "string",
        "LoungeSupplement": true,
        "Currency": {
            "name": "string",
            "rate": 0
        }
    }
]
```

GET:  
api/flight/currencies  
=> List of available currencies (without rate)
```json
["USD", "HKD"]
```

# Currency converting service

There is a service that we'll use to make the currency convertion.  
The server listen on port `7861` on all interfaces.

GET:  
/<currency>  
=> The body contain a float that is the rate of the <currency>

GET:  
/currencies  
=> List of available currencies (without rate)
```json
["USD", "HKD"]
```

# External service

The external service run on the port `7863` on all interfaces.

# The out

it's user as a end point for others to sell out tickets.  
It listen on poirt `7864` on all interfaces.

GET:  
/alive/
=> check if the server is alive
`Working using Go 17`

GET:  
/flights/
=> return an array of flights

```json
[
    {
        "code": 1,
        "from": "DTW",
        "to": "JFK",
        "available_places": 459,
        "price": 300
    },
    {
        "code": 2,
        "from": "CDG",
        "to": "DTW",
        "available_places": 231,
        "price": 500
    }
]
```
