import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FlightListPageComponent } from './pages/flight-list-page/flight-list-page.component';
import { FlightRoutingModule } from './flight-routing.module';
import { UiModule } from '../ui/ui.module';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzDatePickerModule } from 'ng-zorro-antd/date-picker';

@NgModule({
  declarations: [
    FlightListPageComponent
  ],
  imports: [
    CommonModule,
    FlightRoutingModule,
    UiModule,
    NzFormModule,
    NzSelectModule,
    // NzDatePickerModule
  ]
})
export class FlightModule { }
