import { HttpClient, HttpParams } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Book } from '../models/book.models';
import { ApiFlight, Flight } from '../models/flight.models';

@Injectable({
  providedIn: 'root'
})
export class FlightApiService {
  private baseUrl: string;

  constructor(
    private httpClient: HttpClient,
  ) {
    this.baseUrl = `http://localhost:7860`;
  }

  public getFlights(): Observable<ApiFlight> {
    return this.httpClient.get<ApiFlight>(`${this.baseUrl}/flights`);
  }

  public bookTicket(book: Book): Observable<Book> {
    return this.httpClient.post<Book>(`${this.baseUrl}/book`, book).pipe(catchError(this.handleError('addHero', book))
    );;
  }
  
  handleError(arg0: string, book: Book): (err: any, caught: Observable<Book>) => import("rxjs").ObservableInput<any> {
    throw new Error('Method not implemented.');
  }
}
