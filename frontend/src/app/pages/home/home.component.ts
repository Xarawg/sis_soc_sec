import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { ModalChangePasswordComponent } from 'src/app/modal-change-password/modal-change-password.component';
import { ModalOpenOrderComponent } from 'src/app/modal-open-order/modal-open-order.component';

@Component({
  selector: 'home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent {

  constructor(private router: Router,
    private dialog: MatDialog) {
  }

  goToAdmin(): void {
    this.router.navigateByUrl('/admin-start');
  }

  goToOperator(): void {
    this.router.navigateByUrl('/operator-start');
  }

  goToChangePassword(): void {
    this.dialog.open(ModalChangePasswordComponent, {
      height: "calc(60% - 100px)",
      width: "calc(50% - 100px)",
      maxWidth: "100%",
      maxHeight: "100%",
      data: {}
    });
  }
}
