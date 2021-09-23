import { getCurrencySymbol } from '@angular/common';
import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { first } from 'rxjs/operators';
import { CurrenciesApiService } from 'src/app/core/api/currency-api.service';
import { FlightApiService } from 'src/app/core/api/flight-api.service';
import { Airport } from 'src/app/core/models/airport.model';
import { Ticket, TicketOptions } from 'src/app/core/models/book.models';
import { Flight } from 'src/app/core/models/flight.models';

@Component( {
  templateUrl: './flight-list-page.component.html',
  styleUrls: ['./flight-list-page.component.less'],
  encapsulation: ViewEncapsulation.None
} )
export class FlightListPageComponent implements OnInit {

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
    this.currenciesList = ['EUR']
    this.forms = new Array<FormGroup>();
    this.initializeForm();
  }

  initializeForm(): void {
    this.bookForm = this._formBuilder.group( {
      currency: ['EUR', Validators.required],
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
        return f.from;
      } );
      this.airports = this.airports.filter( ( value, index ) => this.airports.indexOf( value ) === index );
    } );
  }

  getCurrencies(): void {
    this._currenciesApiService.getCurrencies().pipe( first() ).subscribe( ( res: Array<string> ) => {
      res.forEach( currency => {
        this.currenciesList.push(currency)
      })
    });
  }

  getRate(): number {
    if (this.bookForm.get('currency')?.value !== 'EUR') {
      this._currenciesApiService.getCurrencyRate( this.bookForm.get( 'currency' )?.value ).pipe( first() ).subscribe( ( res: number ) => {
        this.rate = res
      } );
      return this.rate;
    } else {
      return this.rate = 1
    }
  }

  submitForm() {
    let ids: Array<TicketOptions> = [];
    this.forms.forEach( ticket => {
      const flightId: TicketOptions = {
        code: this.flightsList.find( f =>
        f.from == ticket.get( 'from' )?.value && f.to == ticket.get( 'to' )?.value
      )!.flightCode
    }
      ids.push( flightId );
      if ( ticket.get( 'oneWay' )?.value ) {
        const flightId: TicketOptions = {
          code: this.flightsList.find( f =>
          f.from == ticket.get( 'to' )?.value && f.to == ticket.get( 'from' )?.value
        )!.flightCode
          }
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
 
  getOptions(ticket: FormGroup) {
    return this.flightsList.find(f =>
        f.from == ticket.get('from')?.value && f.to == ticket.get('to')?.value
    )!.options;
}

  getTotal() {
    let flightPrice = 0;
    let numberOfLounge = 0;
    this.forms.forEach( ticket => {
      const ticketPrice = this.flightsList.find( f =>
        f.from == ticket.get( 'from' )?.value
        && f.to == ticket.get( 'to' )?.value
      )?.price as number;

      if ( ticket.get( 'oneWay' )?.value ) {
        flightPrice += (ticketPrice * 2) * 0.9
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
