import { Airport } from "./airport.model";

export interface Flight {
    flightCode: string;
    from: Airport;
    to: Airport;
    availablePlaces: number;
    totalPlaces: number;
    price: number;
}