import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { OperatorStartComponent } from './pages/operator-start/operator.start.component';
import { OrderTableComponent } from './pages/orders-table/orders-table.component';
import { OperatorRegisterComponent } from './pages/operator-register/operator.register.component';
import { UsersTableComponent } from './pages/users/users-table.component';
import { AdminRegisterComponent } from './pages/admin-register/admin.register.component';
import { AdminStartComponent } from './pages/admin-start/admin.start.component';
import { HomeComponent } from './pages/home/home.component';
import { AuthGuard } from './guards/auth.guard';

const routes: Routes = [
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  { path: 'home', component: HomeComponent },
  { path: 'operator-start', component: OperatorStartComponent },
  { path: 'operator-register', component: OperatorRegisterComponent },
  { path: 'orders-table', component: OrderTableComponent, canActivate: [AuthGuard] },
  { path: 'admin-start', component: AdminStartComponent },
  { path: 'admin-register', component: AdminRegisterComponent },
  { path: 'users-table', component: UsersTableComponent, canActivate: [AuthGuard] }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
