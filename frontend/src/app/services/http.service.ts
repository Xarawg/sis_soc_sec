import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs';
import { environment } from '../environments/environment';
import { User } from '../interfaces/user';
import { Order } from '../interfaces/order';
import { operatorOrderGetModel } from '../controllerDTO/operatorOrderGetModel';

@Injectable({
  providedIn: 'root'
})
export class HttpService {

  constructor(private http: HttpClient) { 

  }

  /** Получение списка пользователей */
  getUsers() {
    return this.http.get<User[]>(`${environment.apiUrl}/administrator/get-users`);
  }

  /** Получение списка заявок */
  getOrders() {
    const model: operatorOrderGetModel = { 'limitRowCount': 1000, 'limitOffset': 0 };
    return this.http.post<Order[]>(`${environment.apiUrl}/operator/get-orders`, model);
  }
}
