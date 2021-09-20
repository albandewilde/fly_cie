import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FlightListPageComponent } from './pages/flight-list-page/flight-list-page.component';

const routes: Routes = [
  {
    component: FlightListPageComponent,
    path: 'flight'
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class FlightRoutingModule { }
