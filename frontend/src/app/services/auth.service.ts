import { HttpClient } from '@angular/common/http';
import { Injectable, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, Observable, map, of } from 'rxjs';
import { environment } from '../environments/environment';
import { UserData } from '../interfaces/user.data';
import { UserAuth } from '../interfaces/userAuth';


@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private userSubject: BehaviorSubject<UserData | null>;
  public user: Observable<UserData | null>;

  constructor(private router: Router, private http: HttpClient) {
    const uData = localStorage.getItem('userData');
    this.userSubject = new BehaviorSubject<UserData | null>(
      uData != null ? JSON.parse(uData) : null
    );
    this.user = this.userSubject.asObservable();
  }

  public get userValue(): any {
    return this.userSubject.value;
  }

  login(authModel: UserAuth) {
    return this.http.post<any>(`${environment.apiUrl}/user/auth`, authModel)
        .pipe(
          map((result) => {
            if (result.hasErrors) {
              throw result.error;
            }
            this.userSubject.next(result.value);
            localStorage.setItem('userData', JSON.stringify(result.value));
            return result.value;
          })
        );
  }

  logout() {
    // remove user from local storage to log user out
    localStorage.removeItem('user');
    this.userSubject.next(null);
    this.router.navigate(['/login']);
  }
}
