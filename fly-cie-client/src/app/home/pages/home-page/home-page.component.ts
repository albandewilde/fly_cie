import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { Router } from '@angular/router';
import { first } from 'rxjs/operators';
import { FlightApiService } from 'src/app/core/api/flight-api.service';
import { Flight } from 'src/app/core/models/flight.models';

@Component( {
  templateUrl: './home-page.component.html',
  styleUrls: ['./home-page.component.less'],
  encapsulation: ViewEncapsulation.None
} )
export class HomePageComponent implements OnInit {

  public router: Router;
  public flightsList: Array<Flight>;
  public airports: Array<string>;


  constructor (
    private _flightApiService: FlightApiService,
    router: Router
  ) {
    this.router = router
  }

  ngOnInit(): void {
    this.getFlights();

  }

  toResa() {
    this.router.navigate( ['/flight'] );
  }

  getFlights(): void {
    this._flightApiService.getFlights().pipe( first() ).subscribe( ( res: Array<Flight> ) => {
      this.flightsList = [...res];
      // this.airports = res.map( f => f.to );
      // this.airports = this.airports.filter( ( value, index ) => this.airports.indexOf( value ) === index );
    } );
  }

}
