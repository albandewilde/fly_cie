import { HttpClient, HttpParams } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Flight } from '../models/flight.models';

@Injectable({
  providedIn: 'root'
})
export class StatsApiService {
  private baseUrl: string;

  constructor(
    private httpClient: HttpClient,
  ) {
    this.baseUrl = `http://localhost:7860/stats`;
  }

  public getFlights(): Observable<Flight> {
    return this.httpClient.get<Flight>(`${this.baseUrl}`);
  }
}
