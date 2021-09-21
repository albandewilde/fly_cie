# fly_cie

School project for architecture course

# Backend endpoints

The server run on ther port `7860` on all interfaces

GET :  
/flights   
=> returns an Array of Flight
```json
{
    "id": 1,
    "from": "DTW",
    "to": "CDG",
    "available_places": 700,
    "total_places": 700,
    "price": 700
}
```

POST :  
/book   
=> body contains
```json
{
    "last_name": "string",
    "first_name": "string",
    "nationality": "string",
    "flight_ids": [1, 2, 3],
    "lounge_supplement": true,
    "currency": "string"
}
```
=> returns the created ticket
```json
{
    "last_name": "string",
    "first_name": "string",
    "nationality": "string",
    "flight_id": 15,
    "price": 300,
    "creation_date": "string",
    "lounge_supplement": true,
    "rate": 1,
    "currency": "string"
}
```

GET:  
/currencies  
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
