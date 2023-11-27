import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent {

  constructor(private router: Router) {

  }

  goToAdmin(): void {
    this.router.navigateByUrl('/admin-start');
  }

  goToOperator(): void {
    this.router.navigateByUrl('/operator-start');
  }
}
