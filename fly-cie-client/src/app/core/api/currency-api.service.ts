import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { Currencies } from "../models/currencies.models";

@Injectable( {
    providedIn: 'root'
  } )
  export class CurrenciesApiService {
    private baseUrl: string;
  
    constructor (
      private httpClient: HttpClient,
    ) {
      this.baseUrl = `http://localhost:7861`;
    }
  
    public getCurrencies(): Observable<Array<string>> {
      return this.httpClient.get<Array<string>>( `${this.baseUrl}/currencies` );
    }
  }
  