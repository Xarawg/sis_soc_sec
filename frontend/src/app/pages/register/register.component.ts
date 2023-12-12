import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { UserRoles } from 'src/app/enums/userRoles';
import { UserStates } from 'src/app/enums/userStates';
import { User } from 'src/app/interfaces/user';
import { UserAuth } from 'src/app/interfaces/userAuth';
import { UserRegistrationInputModel } from 'src/app/interfaces/userRegistrationInputModel';
import { ModalComponent } from 'src/app/modal/modal.component';
import { HttpService } from 'src/app/services/http.service';

@Component({
  selector: 'register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class AdminRegisterComponent implements OnInit {
  Roles: any = ['Admin', 'Author', 'Reader'];

  form: FormGroup;

  constructor(
    private router: Router,
    private dialog: MatDialog,
    private formBuilder: FormBuilder,
    private httpService: HttpService,

  ) {

  }
  ngOnInit(): void {
    this.form = this.formBuilder.group({
      userName: ['', [Validators.required]],
      fio: ['', [Validators.required]],
      organization: ['', [Validators.required]],
      inn: ['', [Validators.required]],
      address: ['', [Validators.required]],
      email: ['', [Validators.required]],
      phoneNumber: ['', [Validators.required]],
      password: new FormControl('')
    });
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
    this.httpService.registrationByUser(model).subscribe( (data:any)=> {
      const result = data.value != null && data.value != undefined ? true : false;
      if (result == true) {
        this.dialog.open(ModalComponent, {
          width: '550',
          data: {
            modalText: 'Регистрация прошла успешно.'
          }
        });
        this.router.navigateByUrl('/admin-start');
      } else {
        this.dialog.open(ModalComponent, {
          width: '550',
          data: {
            modalText: 'Произошла ошибка регистрации, такой пользователь уже зарегистрирован.'
          }
        });
      }
    });
    }
    else {
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
