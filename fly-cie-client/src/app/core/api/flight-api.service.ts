import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Ticket } from '../models/book.models';
import { Flight } from '../models/flight.models';

@Injectable( {
  providedIn: 'root'
} )
export class FlightApiService {
  private baseUrl: string;

  constructor (
    private httpClient: HttpClient,
  ) {
    this.baseUrl = `http://localhost:5000/api`;
  }

  public getFlights(): Observable<Array<Flight>> {
    return this.httpClient.get<Array<Flight>>( `${this.baseUrl}/flight/getFlights` );
  }

  public bookTicket( book: Ticket ) {
    return this.httpClient.post( `${this.baseUrl}/flight/bookTicket`, book );
  }
}
