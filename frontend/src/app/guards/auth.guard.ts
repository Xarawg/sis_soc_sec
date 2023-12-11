
import { Injectable } from '@angular/core';
import { Router, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Injectable({ providedIn: 'root' })
export class AuthGuard  {
    constructor(
        private router: Router,
        private authenticationService: AuthService
    ) { }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        const user = this.authenticationService.userTokenValue;
        console.log('user ', user)
        if (user) {
            // logged in so return true
            console.log('user ', true)
            return true;
        }

        // not logged in so redirect to login page with the return url
            console.log('user ', false)
        this.router.navigate(['/home'], { queryParams: { returnUrl: state.url } });
        return false;
    }
}
