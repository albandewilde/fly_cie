import { HttpClient, HttpParams } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Ticket } from '../models/book.models';
import { ApiFlight, Flight } from '../models/flight.models';

@Injectable( {
  providedIn: 'root'
} )
export class FlightApiService {
  private baseUrl: string;

  constructor (
    private httpClient: HttpClient,
  ) {
    this.baseUrl = `http://localhost:7860`;
  }

  public getFlights(): Observable<ApiFlight> {
    return this.httpClient.get<ApiFlight>( `${this.baseUrl}/flights` );
  }

  public bookTicket( book: Ticket ) {
    return this.httpClient.post( `${this.baseUrl}/book`, book );
  }
}
