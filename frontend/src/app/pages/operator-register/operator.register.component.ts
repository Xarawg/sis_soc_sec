import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { UserRoles } from 'src/app/enums/userRoles';
import { UserStates } from 'src/app/enums/userStates';
import { User } from 'src/app/interfaces/user';
import { UserAuth } from 'src/app/interfaces/userAuth';
import { ModalComponent } from 'src/app/modal/modal.component';
import { FakeBackendService } from 'src/app/services/fake-backend.service';

@Component({
  selector: 'operator-register',
  templateUrl: './operator.register.component.html',
  styleUrls: ['./operator.register.component.scss']
})
export class OperatorRegisterComponent implements OnInit {

  form: FormGroup;

  constructor(private backendService: FakeBackendService,
    private router: Router,
    private dialog: MatDialog,
    private formBuilder: FormBuilder
  ) {

  }
  ngOnInit(): void {
    this.form = this.formBuilder.group({
      login: ['', [Validators.required]],
      fio: ['', [Validators.required]],
      organization: ['', [Validators.required]],
      innOrganization: ['', [Validators.required]],
      addressOrganization: ['', [Validators.required]],
      email: ['', [Validators.required]],
      phone: ['', [Validators.required]],
      password: new FormControl('')
    });
  }

  submit() {
    if (this.form.valid) {
      const user: UserAuth = {
        login: this.form.value.login,
        password: this.form.value.password
      }
      const result = this.backendService.registerOperator(user);
      if (result == true) {
        this.dialog.open(ModalComponent, {
          width: '550',
          data: {
            modalText: 'Регистрация прошла успешно.'
          }
        });
        this.router.navigateByUrl('/operator-start');
      } else {
        this.dialog.open(ModalComponent, {
          width: '550',
          data: {
            modalText: 'Произошла ошибка регистрации, такой пользователь уже зарегистрирован.'
          }
        });
      }
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

  backToOperatorStart() {
    this.router.navigateByUrl('/operator-start');
  }
}
