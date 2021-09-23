export interface Ticket {
    lastName: string,
    firstName: string,
    nationality: string,
    flightCodes: Array<TicketOptions>,
    loungeSupplement: boolean,
    currency: string
}

export interface TicketOptions {
    code: string,
    options?: Array<string>
}