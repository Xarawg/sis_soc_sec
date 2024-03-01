import { ChangeDetectionStrategy, Component, EventEmitter, Inject, Input, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialog, MatDialogRef } from "@angular/material/dialog";
import { userColumnsConstants } from '../constants/user.columns.constants';
import { FormBuilder, Validators, FormControl, FormGroup } from '@angular/forms';
import { User } from '../interfaces/user';
import { AuthService } from '../services/auth.service';
import { ModalComponent } from '../modal/modal.component';
import { AdminRegistrationInputModel } from '../interfaces/adminRegistrationInputModel';
import { HttpService } from '../services/http.service';
import { Router } from '@angular/router';
import { AdminChangeInputModel } from '../interfaces/adminChangeInputModel';
import { MyErrorStateMatcher } from '../errorStateMatcher/errorStateMatcher';


@Component({
  selector: 's-modal-open-user',
  templateUrl: './modal-open-user.component.html',
  styleUrls: ['./modal-open-user.component.scss'],
  changeDetection: ChangeDetectionStrategy.Default
})
export class ModalOpenUserComponent implements OnInit {
  userColumnNames = userColumnsConstants.labelColumns;
  states = userColumnsConstants.states;

  form: FormGroup;
  file_store: FileList;
  statuses: any;
  roles: any;
  statusPlaceholder: any;

  resultModal = new EventEmitter<boolean>();
  
  /**
   * Отмечает ошибки по кастомной логике. 
   * В текущем виде - подсвечивает поля ошибочными до того, как пользователь их дотронется.
   */
  matcher = new MyErrorStateMatcher();

  constructor(
    @Inject(MAT_DIALOG_DATA) public user: User,
    private dialogRef: MatDialogRef<ModalOpenUserComponent>,
    private formBuilder: FormBuilder,
    private authService: AuthService,
    private httpService: HttpService,
    private dialog: MatDialog,
    private router: Router,
  ) {

  }

  ngOnInit() {
    this.statuses = [
      { value: -2, valueView: "Заблокированный" },
      { value: -1, valueView: "Отклоненный" },
      { value: 0, valueView: "Вновь заведенный пользователь" },
      { value: 1, valueView: "Зарегистрированный пользователь" }
    ];
    this.setStatusPlaceholder();
    this.roles = this.getAvailableRolesForChange();
    if (!!this.user && this.user.userName != null) {
      this.form = this.formBuilder.group({
        userName: new FormControl({ value: this.user.userName, disabled: true }),
        userRole: [this.user.userRole, Validators.required],
        fio: [this.user.fio, Validators.required],
        organization: [this.user.organization, Validators.required],
        inn: [this.user.inn, Validators.required],
        address: [this.user.address, Validators.required],
        email: [this.user.email, Validators.required],
        phoneNumber: [this.user.phoneNumber, Validators.required],
        state: new FormControl({ value: this.user.state, disabled: true })
      });
    } else {
      this.form = this.formBuilder.group({
        userName: ['', Validators.required],
        userRole: ['', Validators.required],
        fio: ['', Validators.required],
        organization: ['', Validators.required],
        inn: ['', Validators.required],
        address: ['', Validators.required],
        email: ['', Validators.required],
        phoneNumber: ['', Validators.required],
        state: ['', Validators.required],
        password: ['', Validators.required]
      });
    }
    console.log("this.form ", this.form.value)
    console.log("this.user ", this.user)
  }

  setStatusPlaceholder(): void {
    const res = this.statuses.filter((x: any) => x.value === this.user.state);
    this.statusPlaceholder = res[0];
  }

  /** Получаем строку со значением состояния */
  getState(state: any): string {
    //самое младшее значение в enum имеет значение -2, а данные в массиве states начинаются с 0. Поэтому прибавляем 2.
    const result: number = +state + 2;
    return this.states[result];
  }

  /**
   * Получить список доступных для изменения ролей пользователей
   */
  getAvailableRolesForChange(): any {
    switch (this.authService.userValue.role) {
      case 3:
        return [
          { value: 0, valueView: "Отсутствует" },
          { value: 1, valueView: "Оператор" },
          { value: 2, valueView: "Администратор" }
        ];
      default:
        return [
          { value: 0, valueView: "Отсутствует" },
          { value: 1, valueView: "Оператор" },
        ];
    }
  }

  declineUser(): void {
    console.log('declineUser')
  }

  registerUser() {
    if (this.form.valid) {
      const model: AdminRegistrationInputModel = {
        userName: this.form.value.userName,
        email: this.form.value.email,
        phoneNumber: this.form.value.phoneNumber,
        fio: this.form.value.fio,
        organization: this.form.value.organization,
        inn: this.form.value.inn,
        role: this.form.value.userRole.value,
        state: this.form.value.state,
        address: this.form.value.address,
        password: this.form.value.password
      }
      this.httpService.registrationByAdmin(model).subscribe((data: any) => {
        const result = data.value != null && data.value != undefined ? true : false;
        if (result == true) {
          this.dialog.open(ModalComponent, {
            width: '550',
            data: {
              modalText: 'Регистрация прошла успешно.'
            }
          });
        } else {
          this.dialog.open(ModalComponent, {
            width: '550',
            data: {
              modalText: 'Произошла ошибка регистрации.'
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

  /**
   * Сохранить изменение пользователя.
   */
  changeUser(userName: any) {
    if (this.form.valid) {
      const model: AdminChangeInputModel = {
        userName: userName,
        email: this.form.value.email,
        phoneNumber: this.form.value.phoneNumber,
        fio: this.form.value.fio,
        organization: this.form.value.organization,
        inn: this.form.value.inn,
        role: this.form.value.userRole.value,
        state: this.form.value.state,
        address: this.form.value.address
      }
      console.log('modthis.formel ', this.form.value)
      console.log('model ', model)
      this.httpService.changeUserByAdmin(model).subscribe((data: any) => {
        const result = data.value != null && data.value != undefined ? true : false;
        if (result == true) {
          this.dialog.open(ModalComponent, {
            width: '550',
            data: {
              modalText: 'Изменение пользователя прошло успешно.'
            }
          });
        } else {
          this.dialog.open(ModalComponent, {
            width: '550',
            data: {
              modalText: 'Произошла ошибка изменения пользователя.'
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

  blockUser(): void {
    console.log('blockUser')
  }

  submit() {
    if (this.form.valid) {
    }
  }

  cancel() {
    this.resultModal.emit(false);
    this.dialogRef.close();
  }

}
