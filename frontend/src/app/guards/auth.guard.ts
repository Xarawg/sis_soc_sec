
import { Injectable } from '@angular/core';
import { Router, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { MatDialog } from '@angular/material/dialog';
import { ModalComponent } from '../modal/modal.component';

@Injectable({ providedIn: 'root' })
export class AuthGuard {
    constructor(
        private router: Router,
        private authService: AuthService,
        private dialog: MatDialog,
    ) { }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        const user = this.authService.userValue;
        if(!!user && route.data['roles'].includes(user.role)) {
            return true
        } else {
            this.router.navigate(['/home'], { queryParams: { returnUrl: state.url } });
            this.dialog.open(ModalComponent, {
              width: '550',
              data: {
                modalText: "Недостаточно прав."
              }
            });
            return false;
        }
    }
}
