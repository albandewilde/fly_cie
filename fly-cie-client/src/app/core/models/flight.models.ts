import { User } from "./user.models";

export interface Flight {
    id: number,
    from: string,
    to: string,
    available_places: number,
    total_places: number,
    price: number
}

export interface ApiFlight {
    flights: Array<Flight>
}