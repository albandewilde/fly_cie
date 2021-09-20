import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomePageComponent } from './pages/home-page/home-page.component';

const routes: Routes = [
  {
    component: HomePageComponent,
    path: '',
    pathMatch: 'full',
    children: [
      {
        path: 'flight',
        loadChildren: () => import( '../flight/flight.module' ).then( m => m.FlightModule )
      }
    ]
  }
];

@NgModule( {
  imports: [RouterModule.forChild( routes )],
  exports: [RouterModule]
} )
export class HomeRoutingModule { }
