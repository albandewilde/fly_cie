import { Component, Input, OnInit, ViewEncapsulation } from "@angular/core";
import { FormGroup } from "@angular/forms";
import { first } from "rxjs/operators";
import { FlightApiService } from "src/app/core/api/flight-api.service";
import { Airport } from "src/app/core/models/airport.model";
import { Flight } from "src/app/core/models/flight.models";

@Component({
    selector: 'app-book-form',
    templateUrl: './book-form.component.html',
    styleUrls: ['./book-form.component.less'],
    encapsulation: ViewEncapsulation.None
})
export class BookFormComponent implements OnInit {

  @Input() ticketForm: FormGroup;
  @Input() ticketNumber: number;
  @Input() airports: Array<string>;
  public title: string;

    constructor() { }

    ngOnInit(): void {
        this.title = "Billet " + (this.ticketNumber + 1) ;
    }
}