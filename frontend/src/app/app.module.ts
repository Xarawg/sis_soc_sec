import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { OrderTableComponent } from './pages/orders-table/orders-table.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HttpClient, HTTP_INTERCEPTORS } from '@angular/common/http';
import { MaterialModule } from './material.module';
import { OperatorStartComponent } from './pages/operator-start/operator.start.component';
import { ModalComponent } from './modal/modal.component';
import { ModalOpenOrderComponent } from './modal-open-order/modal-open-order.component';
import { RegisterComponent } from './pages/register/register.component';
import { AdminStartComponent } from './pages/admin-start/admin.start.component';
import { ModalOpenUserComponent } from './modal-open-user/modal-open-user.component';
import { UsersTableComponent } from './pages/users-table/users-table.component';
import { HomeComponent } from './pages/home/home.component';
import { JwtInterceptor } from './interceptors/jwt.interceptor';
import { ErrorInterceptor } from './interceptors/error.interceptor';
import { ModalChangePasswordComponent } from './modal-change-password/modal-change-password.component';

@NgModule({
  declarations: [
    AppComponent,
    OperatorStartComponent,
    OrderTableComponent,
    ModalComponent,
    ModalOpenOrderComponent,
    ModalChangePasswordComponent,
    ModalOpenUserComponent,
    RegisterComponent,
    AdminStartComponent,
    UsersTableComponent,
    HomeComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    ReactiveFormsModule,
    MaterialModule
  ],
  providers: [{ provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true }],
  bootstrap: [AppComponent]
})
export class AppModule { }
