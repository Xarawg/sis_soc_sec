import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { BehaviorSubject, Observable, map } from 'rxjs';
import { environment } from '../environments/environment';
import { User } from '../interfaces/user';
import { Order } from '../interfaces/order';
import { operatorOrderGetModel } from '../controllerDTO/operatorOrderGetModel';
import { AdminRegistrationInputModel } from '../interfaces/adminRegistrationInputModel';
import { UserAuth } from '../interfaces/userAuth';
import { UserRegistrationInputModel } from '../interfaces/userRegistrationInputModel';
import { Docscan } from '../interfaces/docscan';
import { OperatorGetDocscanModel } from '../interfaces/operatorGetDocscanModel';
import { OperatorChangeOrderInputModel } from '../interfaces/operatorChangeOrderInputModel';
import { OperatorProcessingOrderInputModel } from '../interfaces/operatorProcessingOrderInputModel';
import { AdminChangeInputModel } from '../interfaces/adminChangeInputModel';


@Injectable({
  providedIn: 'root'
})
export class HttpService {
  uploadFormData$ = new BehaviorSubject<FormData | null>(null);
  
  constructor(private http: HttpClient) { 
  }

  /** Получение списка пользователей */
  getUsers() : any {
    return this.http.get<User[]>(`${environment.apiUrl}/administrator/get-users`);
  }

  /** Получение списка заявок */
  getOrders() : any {
    const model: operatorOrderGetModel = { 'limitRowCount': 1000, 'limitOffset': 0 };
    return this.http.post<Order[]>(`${environment.apiUrl}/operator/get-orders`, model);
  }

  /** Регистрация пользователя */
  registrationByUser(model: UserRegistrationInputModel) {
    return this.http.post<boolean>(`${environment.apiUrl}/user/register`, model);
  }

  /** Регистрация пользователя админом */
  registrationByAdmin(model: AdminRegistrationInputModel) {
    return this.http.post<boolean>(`${environment.apiUrl}/administrator/register`, model);
  }

  /** Изменение пользователя админом */
  changeUserByAdmin(model: AdminChangeInputModel) {
    return this.http.post<boolean>(`${environment.apiUrl}/administrator/change-user`, model);
  }

  /** Изменение пароля пользователя админом */
  changeUserPasswordByAdmin(model: UserAuth) {
    return this.http.post<boolean>(`${environment.apiUrl}/administrator/change-user-password`, model);
  }

  /** Получение файла из заявки */
  public getOrderFileByIdDoc(model: OperatorGetDocscanModel): Observable<any> {  
    let url = `${environment.apiUrl}/operator/get-order-document-by-id`;  
    return this.http.post(url, model, {
      responseType: "blob",
      headers: new HttpHeaders().append("Content-Type", "application/json")
    });
  }

  /** Создание новой заявки оператором */
  createOrder(formData: FormData) {
    if (!!formData) {
      return this.http.post<any>(`${environment.apiUrl}/operator/create-order`, formData);
    } else {
      return null
    }
  }

  /** Изменение заявки оператором */
  changeOrder(model: OperatorChangeOrderInputModel) {
    return this.http.post<boolean>(`${environment.apiUrl}/operator/change-order`, model);
  }

  /** Изменение заявки оператором - один роут для всех видов простых изменений */
  processingOrder(model: OperatorProcessingOrderInputModel) {
    return this.http.post<boolean>(`${environment.apiUrl}/operator/processing-order`, model);
  }
}
