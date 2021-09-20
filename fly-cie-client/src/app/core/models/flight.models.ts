import { User } from "./user.models";

export interface Flight {
    id: number,
    from: string,
    to: string,
    price: number
}

export interface Ticket {
    flight: Flight,
    user: User,
    date: Date
}