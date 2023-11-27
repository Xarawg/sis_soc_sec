import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  isOperatorAuthenticated: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  isAdminAuthenticated: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
}
