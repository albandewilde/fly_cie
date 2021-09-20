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
=> body contains
```json
{
    "last_name": "string",
    "first_name": "string",
    "nationality": "string",
    "flight_ids": [1, 2, 3] ,
    "lounge_supplement": true
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
    "lounge_supplement": true
}
```
