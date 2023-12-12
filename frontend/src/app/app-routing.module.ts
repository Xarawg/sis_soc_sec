import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { OperatorStartComponent } from './pages/operator-start/operator.start.component';
import { OrderTableComponent } from './pages/orders-table/orders-table.component';
import { UsersTableComponent } from './pages/users-table/users-table.component';
import { AdminRegisterComponent } from './pages/register/register.component';
import { AdminStartComponent } from './pages/admin-start/admin.start.component';
import { HomeComponent } from './pages/home/home.component';
import { AuthGuard } from './guards/auth.guard';
import { UserRoles } from './enums/userRoles';

const routes: Routes = [
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  { path: 'home', component: HomeComponent },
  { path: 'operator-start', component: OperatorStartComponent },
  { 
    path: 'orders-table', 
    component: OrderTableComponent, 
    canActivate: [AuthGuard],
    data: { roles: [UserRoles.SuperAdmin, UserRoles.Operator]}
   },
  { path: 'admin-start', component: AdminStartComponent },
  { path: 'register', component: AdminRegisterComponent },
  { 
    path: 'users-table', 
    component: UsersTableComponent, 
    canActivate: [AuthGuard],
    data: { roles: [UserRoles.SuperAdmin, UserRoles.Admin]} 
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
