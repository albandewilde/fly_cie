import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FlightListPageComponent } from './pages/flight-list-page/flight-list-page.component';
import { FlightRoutingModule } from './flight-routing.module';
import { UiModule } from '../ui/ui.module';



@NgModule({
  declarations: [
    FlightListPageComponent
  ],
  imports: [
    CommonModule,
    FlightRoutingModule,
    UiModule
  ]
})
export class FlightModule { }
