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
import { AdministratorProcessingUserInputModel } from '../interfaces/AdministratorProcessingUserInputModel';
import { ChangePasswordDTO } from '../interfaces/changePasswordDTO';
import { UserChangePasswordModel } from '../interfaces/userChangePasswordModel';


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
  getOrders() {
    const model: operatorOrderGetModel = { 'limitRowCount': 1000, 'limitOffset': 0 };
    return this.http.post<Order[]>(`${environment.apiUrl}/operator/get-orders`, model);
  }

  /** Регистрация пользователя */
  registrationByUser(model: UserRegistrationInputModel) {
    return this.http.post<boolean>(`${environment.apiUrl}/user/register`, model);
  }

  /** Регистрация пользователя админом */
  createNewUserByAdmin(model: AdminRegistrationInputModel) {
    return this.http.post<boolean>(`${environment.apiUrl}/administrator/create-new-user`, model);
  }

  /** Изменение пользователя админом */
  changeUserByAdmin(model: AdminChangeInputModel) {
    return this.http.post<boolean>(`${environment.apiUrl}/administrator/change-user`, model);
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
    return this.http.post<any>(`${environment.apiUrl}/operator/create-order`, formData);
  }

  /** Изменение заявки оператором */
  changeOrder(model: OperatorChangeOrderInputModel) {
    return this.http.post<boolean>(`${environment.apiUrl}/operator/change-order`, model);
  }

  /** Изменение заявки оператором - один роут для всех видов простых изменений */
  processingOrder(model: OperatorProcessingOrderInputModel) {
    return this.http.post<boolean>(`${environment.apiUrl}/operator/processing-order`, model);
  }

  /** Изменение заявки оператором - один роут для всех видов простых изменений */
  processingUser(model: AdministratorProcessingUserInputModel) {
    return this.http.post<boolean>(`${environment.apiUrl}/administrator/processing-user`, model);
  }

  /** Генерация случайного пароля пользователю через панель администратора,
   * но у пароля ограниченный срок жизни.
   */
  changeUserPasswordByAdmin(model: ChangePasswordDTO) {
    return this.http.post<boolean>(`${environment.apiUrl}/administrator/change-user-password`, model);
  }

  /** Изменить свой пароль будучи авторизованным. */
  changePassword(model: UserChangePasswordModel) {
    return this.http.post<boolean>(`${environment.apiUrl}/user/change-password`, model);
  }
}
