import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FlightListPageComponent } from './pages/flight-list-page/flight-list-page.component';
import { FlightRoutingModule } from './flight-routing.module';
import { UiModule } from '../ui/ui.module';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzDatePickerModule } from 'ng-zorro-antd/date-picker';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NzInputModule } from 'ng-zorro-antd/input';
import { HttpClientModule } from '@angular/common/http';
import { NzCheckboxModule } from 'ng-zorro-antd/checkbox';
import { NzButtonModule } from 'ng-zorro-antd/button';

@NgModule( {
  declarations: [
    FlightListPageComponent
  ],
  imports: [
    CommonModule,
    FlightRoutingModule,
    UiModule,
    FormsModule,
    HttpClientModule,
    ReactiveFormsModule,
    NzButtonModule,
    NzCheckboxModule,
    NzDatePickerModule,
    NzFormModule,
    NzInputModule,
    NzSelectModule,
  ]
} )
export class FlightModule { }
