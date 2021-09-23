import { Airport } from "./airport.model";

export interface Flight {
    flightCode: string;
    from: string;
    to: string;
    availablePlaces: number;
    totalPlaces: number;
    price: number;
    options: Array<FlightOptions>
}

export interface FlightOptions {
    price: number,
    option_type: string
}