import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";

@Injectable( {
  providedIn: 'root'
} )
export class CurrenciesApiService {
  private baseUrl: string;

  constructor (
    private httpClient: HttpClient,
  ) {
    this.baseUrl = `http://localhost:5000/api/flight`;
  }

  public getCurrencies(): Observable<Array<string>> {
    return this.httpClient.get<Array<string>>( `${this.baseUrl}/currencies` );
  }

  public getCurrencyRate(currency: string): Observable<number> {
    return this.httpClient.get<number>(`${this.baseUrl}/getRate?currency=${currency}`);
  }
}
