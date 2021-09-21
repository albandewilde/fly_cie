import { Airport } from "./airport.model";

export interface Flight {
    flightId: number;
    from: Airport;
    to: Airport;
    availablePlaces: number;
    totalPlaces: number;
    price: number;
}