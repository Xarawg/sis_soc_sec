import { HttpClient } from '@angular/common/http';
import { Injectable, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, Observable, map } from 'rxjs';
import { environment } from '../environments/environment';
import { UserData } from '../interfaces/user.data';
import { UserAuth } from '../interfaces/userAuth';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private userSubject: BehaviorSubject<UserData | null>;
  public user: Observable<UserData | null>;
  // isOperatorAuthenticated: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  // isAdminAuthenticated: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);

  // public userData: BehaviorSubject<UserData | null> = new BehaviorSubject<UserData | null>(null);
  // public userData$: Observable<UserData | null>;

  constructor(
      private router: Router,
      private http: HttpClient
  ) {
    const uData = localStorage.getItem('userData');
    this.userSubject = new BehaviorSubject<UserData | null>(uData != null ? JSON.parse(uData) : null);
    this.user = this.userSubject.asObservable();
  }

  public get userValue(): any {
      return this.userSubject.value;
  }

  login(authModel: UserAuth) {
      // return this.http.post<any>(`${environment.apiUrl}/users/authenticate`, { username, password })
      return this.http.post<any>(`${environment.apiUrl}/user/auth`, authModel)
          .pipe(map(user => {
              // store user details and jwt token in local storage to keep user logged in between page refreshes
              localStorage.setItem('userData', JSON.stringify(user));
              this.userSubject.next(user);
              return user;
          }));
  }

  logout() {
      // remove user from local storage to log user out
      localStorage.removeItem('user');
      this.userSubject.next(null);
      console.log('navigate login')
      this.router.navigate(['/login']);
  }
}
  // }
  
  // ngOnInit(): void {
  //   const uData = localStorage.getItem('userData');
  //   console.log('uData != null ? JSON.parse(uData) : null ==== ', uData != null ? JSON.parse(uData) : null)
  //   this.userData.next(uData != null ? JSON.parse(uData) : null)
  //   // this.userData = new BehaviorSubject<UserData | null>(uData != null ? JSON.parse(uData) : null);
  //   // this.userData = new BehaviorSubject(localStorage.getItem('userData'));
  //   this.userData$ = this.userData.asObservable();
  // }

  // public get userDataValue() {
  //     return this.userData.value;
  // }

  // login(authModel: UserAuth) {
  //     return this.http.post<any>(`${environment.apiUrl}/user/auth`, authModel)
  //         .pipe(map(authResult => {
  //             // store userToken details and jwt token in local storage to keep userToken logged in between page refreshes
  //             const userData: UserData = { token: authResult.value.token.token, role: authResult.value.role }
  //             localStorage.setItem('userData', JSON.stringify(userData) );
  //             this.userData.next(userData);
  //             return authResult.value.token;
  //         }));
  // }

  // registration(userTokenname: string, password: string, fio :string){
  //   return this.http.post<any>(`${environment.apiUrl}/user/register`, { userTokenname, password ,fio })
  //   .pipe(map(userToken => {
  //       // store userToken details and jwt token in local storage to keep userToken logged in between page refreshes

  //       return userToken;
  //   }));
  // }
  // logout() {
  //     // remove userToken from local storage to log userToken out
  //     // localStorage.removeItem('userToken');
  //     localStorage.removeItem('userData');
  //     this.userData.next(null);
  //     console.log('logout to home');
  //     this.router.navigate(['/home']);
  // }
