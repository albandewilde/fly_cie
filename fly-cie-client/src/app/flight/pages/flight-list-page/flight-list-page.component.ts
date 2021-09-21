import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { first } from 'rxjs/operators';
import { CurrenciesApiService } from 'src/app/core/api/currency-api.service';
import { FlightApiService } from 'src/app/core/api/flight-api.service';
import { Airport } from 'src/app/core/models/airport.model';
import { Ticket } from 'src/app/core/models/book.models';
import { Flight } from 'src/app/core/models/flight.models';

@Component( {
  templateUrl: './flight-list-page.component.html',
  styleUrls: ['./flight-list-page.component.less'],
  encapsulation: ViewEncapsulation.None
} )
export class FlightListPageComponent implements OnInit {

  private _airportEnum = Airport;
  public flightsList: Array<Flight>;
  public airports: Array<string>;
  public ticket: Ticket;
  public form: FormGroup;
  public currenciesList: Array<string>;

  constructor (
    private _flightApiService: FlightApiService,
    private _currenciesApiService: CurrenciesApiService,
    private _formBuilder: FormBuilder
  ) {
    this.airports = new Array<string>();
    this.flightsList = new Array<Flight>();
    this.currenciesList = new Array<string>();
    this.initializeForm();
  }

  initializeForm(): void {
    this.form = this._formBuilder.group( {
      currency: ['USD', Validators.required],
      from: ['CDG', Validators.required],
      to: ['JFK', Validators.required],
      lastName: ['toto', Validators.required],
      firstName: ['tata', Validators.required],
      nationality: ['french', Validators.required],
      loungeSupplement: [false, Validators.required]
    } );
  }

  ngOnInit(): void {
    this.getFlights();
    this.getCurrencies();
  }

  getCurrencies(): void {
    this._currenciesApiService.getCurrencies().pipe( first() ).subscribe( ( res: Array<string> ) => {
      this.currenciesList = [...res];
    } );
  }

  getFlights(): void {
    this._flightApiService.getFlights().pipe( first() ).subscribe( ( res: Array<Flight> ) => {
      this.flightsList = [...res];
      this.airports = res.map( f => {
        return this._airportEnum[f.from];
      } );
      this.airports = this.airports.filter( ( value, index ) => this.airports.indexOf( value ) === index );
    } );
  }

  submitForm() {
    let ids: Array<number> = [];
    const flightId = this.flightsList.find( f =>
      this._airportEnum[f.from] == this.form.get( 'from' )?.value && this._airportEnum[f.to] == this.form.get( 'to' )?.value
    )!.flightId;
    ids.push( flightId );

    if ( this.form.get( 'from' )?.value == 'DTW' ) {
      this.form.patchValue( { 'loungeSupplement': false } );
    }

    const newTicket: Ticket = {
      first_name: this.form.get( 'firstName' )?.value,
      last_name: this.form.get( 'lastName' )?.value,
      flight_ids: ids,
      lounge_supplement: this.form.get( 'loungeSupplement' )?.value,
      nationality: this.form.get( 'nationality' )?.value
    };

    console.log( newTicket );
    this._flightApiService.bookTicket( newTicket ).subscribe( res => {
      debugger;
    } );
  }

  getTotal() {
    let flightPrice = this.flightsList.find( f =>
      this._airportEnum[f.from] == this.form.get( 'from' )?.value
      && this._airportEnum[f.to] == this.form.get( 'to' )?.value
    )?.price;
    if ( this.form.get( 'loungeSupplement' )?.value && this.form.get( 'from' )?.value !== 'DTW' ) {
      return flightPrice! + 150 + ' €'
    }
    return flightPrice + ' €';
  }
}
