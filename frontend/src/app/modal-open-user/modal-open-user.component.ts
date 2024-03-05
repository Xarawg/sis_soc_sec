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
import { PreliminaryErrorDetectionStateMatcher } from '../errorStateMatcher/preliminaryErrorDetectionStateMatcher';
import { ModalService } from '../services/modal.service';
import { regExpConstants } from '../constants/regexp.patterns.constants';
import { FormBuilderService } from '../services/form.builder.service';
import { UserStates } from '../enums/userStates';
import { UserRoles } from '../enums/userRoles';
import { AdministratorProcessingUserInputModel } from '../interfaces/AdministratorProcessingUserInputModel';
import { UserProcessingAction } from '../enums/userProcessingAction';
import { ChangePasswordDTO } from '../interfaces/changePasswordDTO';


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

  /**
   * Список ролей, которые может назначить пользователь другому пользователю
   */
  availableRolesToChange: any;

  roles: any;
  statuses: any;
  statusPlaceholder: any;
  rolePlaceholder: any;

  /**
   * Enum состояний пользователя
   */
  userStates = UserStates;

  resultModal = new EventEmitter<boolean>();
  
  /**
   * Отмечает ошибки по кастомной логике. 
   * В текущем виде - подсвечивает поля ошибочными до того, как пользователь их дотронется.
   */
  matcher = new PreliminaryErrorDetectionStateMatcher();

  constructor(
    @Inject(MAT_DIALOG_DATA) public user: User,
    private dialogRef: MatDialogRef<ModalOpenUserComponent>,
    private formBuilder: FormBuilder,
    private authService: AuthService,
    private httpService: HttpService,
    private modalService: ModalService,
    private dialog: MatDialog,
    private formBuilderService: FormBuilderService,
  ) {

  }

  ngOnInit() {
    this.statuses = [
      { value: -2, valueView: "Заблокированный" },
      { value: -1, valueView: "Отклоненный" },
      { value: 0, valueView: "Вновь заведенный пользователь" },
      { value: 1, valueView: "Зарегистрированный пользователь" }
    ];
    this.roles = [
      { value: 0, valueView: "Отсутствует" },
      { value: 1, valueView: "Оператор" },
      { value: 2, valueView: "Администратор" },
      { value: 3, valueView: "Супер администратор" }
    ];
    this.availableRolesToChange = this.getAvailableRolesForChange();
    this.setStatusPlaceholder();
    this.setRolePlaceholder();

    // Генерация полей формы
    if (!!this.user && this.user.userName != null) {
      this.generateFormForDisplayExistingUser();
    } else {
      this.form = this.formBuilderService.generateFormForNewUserRegisterByAdmin();
    }
  }

  /**
   * Генерация формы для отображения полей формы существующего пользователя.
   */
  generateFormForDisplayExistingUser(): void {
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
  }

  setStatusPlaceholder(): void {
    const res = this.statuses.filter((x: any) => x.value === this.user.state);
    this.statusPlaceholder = res[0];
  }

  setRolePlaceholder(): void {
    const res = this.roles.filter((x: any) => x.value === this.authService.userValue.role);
    this.rolePlaceholder = res[0];
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
      case UserRoles.SuperAdmin:
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

  createUser() {
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

      const registration$ = this.httpService.createNewUserByAdmin(model); 
      registration$.subscribe({
        next: (value : any) => {
          if (value.success === true) {
            // Сообщаем таблице, что данные обновились.
            this.modalService.changed.next(value);
            
            this.dialog.open(ModalComponent, {
              width: '550',
              data: {
                modalText: 'Регистрация прошла успешно.'
              }
            });

            // Закрываем модальное окно, т.к. команда выполнена успешно.
            this.dialogRef.close();
          } else {
            this.dialog.open(ModalComponent, {
              width: '550',
              data: {
                modalText: value.error
              }
            });
          }
        },
        error: (errorValue : any) => {
          this.dialog.open(ModalComponent, {
            width: '550',
            data: {
              modalText: errorValue
            }
          });
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

  /**
   * Сохранить изменение пользователя.
   */
  changeUser(user: User) {
    if (this.form.valid) {
      const model: AdminChangeInputModel = {
        userName: user.userName,
        email: this.form.value.email,
        phoneNumber: this.form.value.phoneNumber,
        fio: this.form.value.fio,
        organization: this.form.value.organization,
        inn: this.form.value.inn,
        role: 0,
        state: user.state,
        address: this.form.value.address
      }
      model.role = this.roles.filter( (x : any) => x.valueView == this.form.value.userRole)[0].value;
      
      const change$ = this.httpService.changeUserByAdmin(model); 
      change$.subscribe({
        next: (value : any) => {
          if (value.success === true) {
            // Сообщаем таблице, что данные обновились.
            this.modalService.changed.next(value);
            
            this.dialog.open(ModalComponent, {
              width: '550',
              data: {
                modalText: 'Изменение пользователя прошло успешно.'
              }
            });

            // Закрываем модальное окно, т.к. команда выполнена успешно.
            this.dialogRef.close();
          } else {
            this.dialog.open(ModalComponent, {
              width: '550',
              data: {
                modalText: value.error
              }
            });
          }
        },
        error: (errorValue : any) => {
          this.dialog.open(ModalComponent, {
            width: '550',
            data: {
              modalText: errorValue
            }
          });
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

  blockUser(): void {
    const model: AdministratorProcessingUserInputModel = {
      id: this.user.id,
      action: UserProcessingAction.Decline as number
    }

    const process$ = this.httpService.processingUser(model);
    process$.subscribe({
      next: (value : any) => {
        if (value.success === true) {
          // Сообщаем таблице, что данные обновились.
          this.modalService.changed.next(value);

          this.dialog.open(ModalComponent, {
            width: '550',
            data: {
              modalText: 'Пользователь заблокирован успешно.'
            }
          });

          // Закрываем модальное окно, т.к. команда выполнена успешно.
          this.dialogRef.close();
        } else {
          this.dialog.open(ModalComponent, {
            width: '550',
            data: {
              modalText: value.error
            }
          });
        }
      },
      error: (errorValue : any) => {
        this.dialog.open(ModalComponent, {
          width: '550',
          data: {
            modalText: errorValue
          }
        });
      }
    });
  }

  declineUser(): void {
    const model: AdministratorProcessingUserInputModel = {
      id: this.user.id,
      action: UserProcessingAction.Decline
    }

    const process$ = this.httpService.processingUser(model);
    process$.subscribe({
      next: (value : any) => {
        if (value.success === true) {
          // Сообщаем таблице, что данные обновились.
          this.modalService.changed.next(value);

          this.dialog.open(ModalComponent, {
            width: '550',
            data: {
              modalText: 'Пользователь отменён успешно.'
            }
          });

          // Закрываем модальное окно, т.к. команда выполнена успешно.
          this.dialogRef.close();
        } else {
          this.dialog.open(ModalComponent, {
            width: '550',
            data: {
              modalText: value.error
            }
          });
        }
      },
      error: (errorValue : any) => {
        this.dialog.open(ModalComponent, {
          width: '550',
          data: {
            modalText: errorValue
          }
        });
      }
    });
  }

  registerUser(){
    const model: AdministratorProcessingUserInputModel = {
      id: this.user.id,
      action: UserProcessingAction.Register
    }

    const process$ = this.httpService.processingUser(model);
    process$.subscribe({
      next: (value : any) => {
        if (value.success === true) {
          // Сообщаем таблице, что данные обновились.
          this.modalService.changed.next(value);

          this.dialog.open(ModalComponent, {
            width: '550',
            data: {
              modalText: 'Пользователь зарегистрирован успешно.'
            }
          });

          // Закрываем модальное окно, т.к. команда выполнена успешно.
          this.dialogRef.close();
        } else {
          this.dialog.open(ModalComponent, {
            width: '550',
            data: {
              modalText: value.error
            }
          });
        }
      },
      error: (errorValue : any) => {
        this.dialog.open(ModalComponent, {
          width: '550',
          data: {
            modalText: errorValue
          }
        });
      }
    });
  }

  changeUserPassword(user: User){
    const randomString = Math.random()                        // Сгенерируется случайное число, например: 0.123456
                                      .toString(36)          // Конвертируется в кодировку base-36, например: "0.4fzyo82mvyr"
                                                 .slice(-8);// Обрежутся последние 8 символов: "yo82mvyr"
    const model: ChangePasswordDTO = {
      userName: user.userName,
      password: randomString
    }

    const process$ = this.httpService.changeUserPasswordByAdmin(model);
    process$.subscribe({
      next: (value : any) => {
        if (value.success === true) {
          this.dialog.open(ModalComponent, {
            width: '550',
            data: {
              modalText: `Пароль успешно сгенерирован. Перепишите его: ${ randomString }`
            }
          });

          // Закрываем модальное окно, т.к. команда выполнена успешно.
          this.dialogRef.close();
        } else {
          this.dialog.open(ModalComponent, {
            width: '550',
            data: {
              modalText: value.error
            }
          });
        }
      },
      error: (errorValue : any) => {
        this.dialog.open(ModalComponent, {
          width: '550',
          data: {
            modalText: errorValue
          }
        });
      }
    });
  }

  submit() {
  }

  cancel() {
    this.resultModal.emit(false);
    this.dialogRef.close();
  }

}
