import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, Observable, map } from 'rxjs';
import { environment } from '../environments/environment';
import { UserToken } from '../interfaces/user.token';
import { UserAuth } from '../interfaces/userAuth';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  isOperatorAuthenticated: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  isAdminAuthenticated: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);

  private userTokenSubject: BehaviorSubject<UserToken | null>;
  public userToken: Observable<UserToken | null>;

  constructor(
      private router: Router,
      private http: HttpClient
  ) {
      this.userTokenSubject = new BehaviorSubject(JSON.parse(localStorage.getItem('userToken')!));
      this.userToken = this.userTokenSubject.asObservable();
  }

  public get userTokenValue() {
      return this.userTokenSubject.value;
  }

  login(authModel: UserAuth) {
    console.log('login')
      return this.http.post<any>(`${environment.apiUrl}/user/auth`, authModel)
          .pipe(map(authResult => {
              // store userToken details and jwt token in local storage to keep userToken logged in between page refreshes
              localStorage.setItem('userToken', JSON.stringify(authResult.value.token.token));
              this.userTokenSubject.next(authResult.value.token.token);
              console.log('token ', authResult.value.token.token)
              return authResult.value.token.token;
          }));
  }
  registration(userTokenname: string, password: string, fio :string){
    return this.http.post<any>(`${environment.apiUrl}/user/register`, { userTokenname, password ,fio })
    .pipe(map(userToken => {
        // store userToken details and jwt token in local storage to keep userToken logged in between page refreshes

        return userToken;
    }));
  }
  logout() {
      // remove userToken from local storage to log userToken out
      localStorage.removeItem('userToken');
      this.userTokenSubject.next(null);
      console.log('logout to home');
      this.router.navigate(['/home']);
  }
}
