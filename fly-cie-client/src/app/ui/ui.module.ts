import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavBarComponent } from './components/nav-bar/nav-bar.component';
import { NzMenuModule } from 'ng-zorro-antd/menu';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzDropDownModule } from 'ng-zorro-antd/dropdown';
import { NzIconModule } from 'ng-zorro-antd/icon';


@NgModule({
  declarations: [
    NavBarComponent
  ],
  imports: [
    CommonModule,
    NzMenuModule,
    NzButtonModule,
    NzDropDownModule,
    NzIconModule
  ],
  exports: [
    NavBarComponent
  ]
})
export class UiModule { }
