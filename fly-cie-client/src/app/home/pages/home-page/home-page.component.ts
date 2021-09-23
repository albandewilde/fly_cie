import { isNull } from '@angular/compiler/src/output/output_ast';
import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { Router } from '@angular/router';
import { first } from 'rxjs/operators';
import { FlightApiService } from 'src/app/core/api/flight-api.service';
import { Airport } from 'src/app/core/models/airport.model';
import { Flight } from 'src/app/core/models/flight.models';

@Component( {
  templateUrl: './home-page.component.html',
  styleUrls: ['./home-page.component.less'],
  encapsulation: ViewEncapsulation.None
} )
export class HomePageComponent implements OnInit {

  public router: Router;
  public flightsList: Array<Flight>;
  public externalFlight: Array<Flight>;
  public airports: Array<string>;
  public _airportEnum = Airport;


  constructor (
    private _flightApiService: FlightApiService,
    router: Router
  ) {
    this.router = router
    this.flightsList = new Array<Flight>();
    this.externalFlight = new Array<Flight>();
  }

  ngOnInit(): void {
    this.getFlights();
  }

  toResa() {
    this.router.navigate( ['/flight'] );
  }

  getFlights(): void {
    this._flightApiService.getFlights().pipe( first() ).subscribe( ( res: Array<Flight> ) => {
      res.forEach( flight => {
        if (flight.options !== null ) {
          this.externalFlight.push(flight)
        }
        else {
          this.flightsList.push(flight);
        }
      })
      this.airports = res.map( f => {
        return f.from;
      } );
      this.airports = this.airports.filter( ( value, index ) => this.airports.indexOf( value ) === index );
    } );
  }
}
