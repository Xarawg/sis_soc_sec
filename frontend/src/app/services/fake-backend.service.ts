import { Injectable } from '@angular/core';
import { UserAuth } from '../interfaces/userAuth';
import { AuthService } from './auth.service';
import { User } from '../interfaces/user';
import { BehaviorSubject } from 'rxjs';
import { Order } from '../interfaces/order';
import { OrderStates } from '../enums/orderStates';
import { OrderTypes } from '../enums/orderTypes';
import { UserRoles } from '../enums/userRoles';
import { UserStates } from '../enums/userStates';

@Injectable({
  providedIn: 'root'
})
export class FakeBackendService {  
  adminsAuthData: Array<UserAuth> = [];
  usersAuthData: Array<UserAuth> = [];

  /** Заявки */
  orders: Array<Order>;
  /** Пользователи */
  usersData: Array<User>;

  constructor(private authService: AuthService) { 
    const user: UserAuth = {
      userName: "test",
      password: "test"
    };
    this.usersAuthData.push(user);
    this.generateOrderData();
    this.generateUserData();
  }

  // loginAdmin(authModel: UserAuth): boolean {
  //   for (let i = 0; i < this.adminsAuthData.length; i ++) {
  //     let user = this.adminsAuthData[i];
  //     if (user.login === authModel.login && user.password === authModel.password) {
  //       this.authService.isAdminAuthenticated.next(true);
  //       return true;
  //     }
  //   };
  //   return false;
  // }

  // registerAdmin(registerModel: UserAuth): boolean {
  //   if (registerModel) {
  //     this.adminsAuthData.push(registerModel);
  //     return true;
  //   }
  //   return false;
  // }

  // loginOperator(authModel: UserAuth): boolean {
  //   for (let i = 0; i < this.usersAuthData.length; i ++) {
  //     let user = this.usersAuthData[i];
  //     if (user.userName === authModel.userName && user.password === authModel.password) {
  //       this.authService.isAdminAuthenticated.next(true);
  //       return true;
  //     }
  //   };
  //   return false;
  // }

  registerOperator(registerModel: UserAuth): boolean {
    if (registerModel) {
      this.usersAuthData.push(registerModel);
      return true;
    }
    return false;
  }

  /**
   * Создание тестового набора данных заявок
   */
  generateOrderData(): void {
    let result: Array<User> = [];
    for (let i = 0; i < 100; i++) {
    let snils = (Math.floor(Math.random() * 899)+100).toString() + '-' + (Math.floor(Math.random() * 899)+100).toString() + '-' + (Math.floor(Math.random() * 899)+100).toString();
    let date = Math.floor(Math.random() * 28).toString() + '.' + Math.floor(Math.random() * 12).toString() + '.' + ((Math.floor(Math.random() * 20)) + 2000).toString();
    let role = Math.floor(Math.random() * 2) as UserRoles;
    let state = Math.floor(Math.random() * 6) as UserStates;
    let phone = '+79' + Math.floor(Math.random() * 99).toString()+ '-' + Math.floor(Math.random() * 999).toString() + '-' + Math.floor(Math.random() * 9999).toString();

      let userModel: User = { 
        userName: 'testLogin',
        role: role,
        fio: 'Тестов Тест Тестович',
        organization: 'Тестовая организация',
        innOrganization: Math.floor(Math.random() * 999999999999).toString(),
        addressOrganization: 'Тестовый адрес тестовой организации',
        email: 'test@mail',
        phone: phone,
        password: 'test',
        state: state
       }
       result.push(userModel);
    }
    /** Заполняем данные с помощью генератора */
    this.usersData = (result);
  }
  
  /**
   * Создание тестового набора данных заявок
   */
  generateUserData(): void {
    let result: Array<Order> = [];
    for (let i = 0; i < 100; i++) {
    let snils = (Math.floor(Math.random() * 899)+100).toString() + '-' + (Math.floor(Math.random() * 899)+100).toString() + '-' + (Math.floor(Math.random() * 899)+100).toString();
    let date = Math.floor(Math.random() * 28).toString() + '.' + Math.floor(Math.random() * 12).toString() + '.' + ((Math.floor(Math.random() * 20)) + 2000).toString();

      let order: Order = { 
        idOrder: i.toString(), 
        date: date,
        state: OrderStates.Newly,
        status: 'Тест',
        snils: snils,
        fio: 'Тест Тестов Тестович',
        contactData: 'тестовые контактные данные',
        type: OrderTypes.RequestForSupportMeasures,
        body: 'тестовое тело запроса',
        documents: null,
        supportMeasures: 'тело ответа из госоргана'
       }
       result.push(order);
    }
    /** Заполняем данные с помощью генератора */
    this.orders = (result);
  }
  
}