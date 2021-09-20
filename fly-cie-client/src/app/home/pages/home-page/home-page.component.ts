import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  templateUrl: './home-page.component.html',
  styleUrls: ['./home-page.component.less'],
  encapsulation: ViewEncapsulation.None
})
export class HomePageComponent implements OnInit {

  public router: Router

  constructor(
    router: Router
  ) { 
    this.router = router 
  }

  ngOnInit(): void {
  }

  toResa() {
    this.router.navigate(['/flight']);
  }

}
