import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { userColumnsConstants } from 'src/app/constants/user.columns.constants';
import { UserRoles } from 'src/app/enums/userRoles';
import { UserStates } from 'src/app/enums/userStates';
import { PreliminaryErrorDetectionStateMatcher } from 'src/app/errorStateMatcher/preliminaryErrorDetectionStateMatcher';
import { User } from 'src/app/interfaces/user';
import { UserAuth } from 'src/app/interfaces/userAuth';
import { UserRegistrationInputModel } from 'src/app/interfaces/userRegistrationInputModel';
import { ModalComponent } from 'src/app/modal/modal.component';
import { FormBuilderService } from 'src/app/services/form.builder.service';
import { HttpService } from 'src/app/services/http.service';

@Component({
  selector: 'register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {
  Roles: any = ['Admin', 'Author', 'Reader'];
  form: FormGroup;
  
  userColumnNames = userColumnsConstants.labelColumns;
  
  /**
   * Отмечает ошибки по кастомной логике. 
   * В текущем виде - подсвечивает поля ошибочными до того, как пользователь их дотронется.
   */
  matcher = new PreliminaryErrorDetectionStateMatcher();

  constructor(
    private router: Router,
    private dialog: MatDialog,
    private formBuilder: FormBuilder,
    private httpService: HttpService,
    private formBuilderService: FormBuilderService
  ) {

  }
  ngOnInit(): void {
    this.form = this.formBuilderService.generateFormForNewUserRegister();
  }

  submit() {
    if (this.form.valid) {
      const model: UserRegistrationInputModel = {            
        userName: this.form.value.userName,
        email: this.form.value.email,
        phoneNumber: this.form.value.phoneNumber,
        fio: this.form.value.fio,
        organization: this.form.value.organization,
        inn: this.form.value.inn,
        address: this.form.value.address,
        password: this.form.value.password
      }

      const registration$ = this.httpService.registrationByUser(model); 
      registration$.subscribe({
        next: (value : any) => {
          if (value.success === true) {     
            this.dialog.open(ModalComponent, {
              width: '550',
              data: {
                modalText: 'Регистрация прошла успешно.'
              }
            });
            this.router.navigateByUrl('/');
          } else {
            this.dialog.open(ModalComponent, {
              width: '550',
              data: {
                modalText: value.error
              }
            });
          }
        }
      });
    } else {
      this.dialog.open(ModalComponent, {
        width: '550',
        data: {
          modalText: 'Заполните все поля формы.'
        }
      });
    }
  }

  backToHome() {
    this.router.navigateByUrl('/home');
  }
}
