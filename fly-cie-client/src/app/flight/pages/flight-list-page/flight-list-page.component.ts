import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { FlightApiService } from 'src/app/core/api/flight-api.service';
import { ApiFlight, Flight } from 'src/app/core/models/flight.models';

@Component({
  templateUrl: './flight-list-page.component.html',
  styleUrls: ['./flight-list-page.component.less'],
  encapsulation: ViewEncapsulation.None
})
export class FlightListPageComponent implements OnInit {

  public flightsList: Array<Flight>
  public airports: Array<string> = [];

  constructor(
    private _flightApiService: FlightApiService
  ) { }

  ngOnInit(): void {
    this._flightApiService.getFlights().subscribe((res: ApiFlight) => {
      this.flightsList = res.flights;
      res.flights.forEach( flight => {
        this.airports.push(flight.to);
      })
      this.airports = this.airports.filter((value, index) => this.airports.indexOf(value) === index);
    });
    console.log(this.airports);
  }

}
