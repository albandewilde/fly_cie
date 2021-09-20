import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { HomeRoutingModule } from './home-routing.module';
import { HomePageComponent } from './pages/home-page/home-page.component';
import { UiModule } from '../ui/ui.module';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { RouterModule } from '@angular/router';


@NgModule( {
  declarations: [
    HomePageComponent
  ],
  imports: [
    CommonModule,
    UiModule,
    NzButtonModule,
    RouterModule,
    HomeRoutingModule,
  ]
} )
export class HomeModule { }
