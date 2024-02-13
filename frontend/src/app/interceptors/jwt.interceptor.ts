import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from '../services/auth.service';
import { environment } from '../environments/environment';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {
    constructor(private authenticationService: AuthService) { }

    /**
     * Добавление токена авторизации.
     */
    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any | null>> {
        const user = this.authenticationService.userValue;
        
        const isLoggedIn = user?.token?.token;
        const isApiUrl = request.url.startsWith(environment.apiUrl);
        if (isLoggedIn && isApiUrl) {
            request = request.clone({
                headers: request.headers.set('Authorization', 'Bearer ' + user.token.token),
            });
        }
        
        return next.handle(request);
    }
}