import { getCurrencySymbol } from '@angular/common';
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
  public bookForm: FormGroup;
  public currenciesList: Array<string>;
  public rate: number;
  public forms: Array<FormGroup>;

  constructor (
    private _flightApiService: FlightApiService,
    private _currenciesApiService: CurrenciesApiService,
    private _formBuilder: FormBuilder
  ) {
    this.airports = new Array<string>();
    this.flightsList = new Array<Flight>();
    this.currenciesList = new Array<string>();
    this.forms = new Array<FormGroup>();
    this.initializeForm();
  }

  initializeForm(): void {
    this.bookForm = this._formBuilder.group( {
      currency: ['USD', Validators.required],
      lastName: [null, Validators.required],
      firstName: [null, Validators.required],
      nationality: [null, Validators.required]
    } );
  }

  initializeTicketForm(): void {
    this.forms.push( this._formBuilder.group( {
      from: ['CDG', Validators.required],
      to: ['JFK', Validators.required],
      oneWay: [false, Validators.required],
      loungeSupplement: [false, Validators.required]
    } ) );
  }

  ngOnInit(): void {
    this.getFlights();
    this.getCurrencies();
    this.initializeTicketForm();
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

  getCurrencies(): void {
    this._currenciesApiService.getCurrencies().pipe( first() ).subscribe( ( res: Array<string> ) => {
      this.currenciesList = [...res];
    } );
  }

  getRate(): number {
    this._currenciesApiService.getCurrencyRate( this.bookForm.get( 'currency' )?.value ).pipe( first() ).subscribe( ( res: number ) => {
      this.rate = res
    } );
    return this.rate;
  }

  submitForm() {
    let ids: Array<string> = [];
    this.forms.forEach( ticket => {
      const flightId = this.flightsList.find( f =>
        this._airportEnum[f.from] == ticket.get( 'from' )?.value && this._airportEnum[f.to] == ticket.get( 'to' )?.value
      )!.flightCode;
      ids.push( flightId );
      if ( ticket.get( 'oneWay' )?.value ) {
        const flightId = this.flightsList.find( f =>
          this._airportEnum[f.from] == ticket.get( 'to' )?.value && this._airportEnum[f.to] == ticket.get( 'from' )?.value
        )!.flightCode;
        ids.push( flightId );
      }
    } )

    if ( this.bookForm.get( 'from' )?.value == 'DTW' ) {
      this.bookForm.patchValue( { 'loungeSupplement': false } );
    }

    const newTicket: Ticket = {
      firstName: this.bookForm.get( 'firstName' )?.value,
      lastName: this.bookForm.get( 'lastName' )?.value,
      flightCodes: ids,
      loungeSupplement: true,
      nationality: this.bookForm.get( 'nationality' )?.value,
      currency: this.bookForm.get( "currency" )?.value
    };

    console.log( newTicket );
    this._flightApiService.bookTicket( newTicket ).subscribe();
  }

  getTotal() {
    let flightPrice = 0;
    let numberOfLounge = 0;
    this.forms.forEach( ticket => {
      const ticketPrice = this.flightsList.find( f =>
        this._airportEnum[f.from] == ticket.get( 'from' )?.value
        && this._airportEnum[f.to] == ticket.get( 'to' )?.value
      )?.price as number;

      if ( ticket.get( 'oneWay' )?.value ) {
        flightPrice += ticketPrice * 2
      } else {
        flightPrice += ticketPrice
      }
      if ( ticket.get( 'loungeSupplement' )?.value ) {
        numberOfLounge++;
      }
    } )

    if ( this.rate ) {
      return ( ( flightPrice! + 150 * numberOfLounge ) * this.rate ).toFixed( 2 ) + ' ' + getCurrencySymbol( this.bookForm.get( 'currency' )?.value, "wide" );
    } else {
      return flightPrice + ' €'
    }
  }
}
