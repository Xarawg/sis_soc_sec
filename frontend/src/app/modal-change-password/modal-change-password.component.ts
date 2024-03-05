import { ChangeDetectionStrategy, Component, EventEmitter, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialog, MatDialogRef } from "@angular/material/dialog";
import { userColumnsConstants } from '../constants/user.columns.constants';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { AuthService } from '../services/auth.service';
import { HttpService } from '../services/http.service';
import { ModalService } from '../services/modal.service';
import { regExpConstants } from '../constants/regexp.patterns.constants';
import { FormBuilderService } from '../services/form.builder.service';
import { UserChangePasswordModel } from '../interfaces/userChangePasswordModel';
import { PasswordConfirmationStateMatcher } from '../errorStateMatcher/passwordConfirmationStateMatcher';
import { ModalComponent } from '../modal/modal.component';


@Component({
  selector: 's-modal-change-password',
  templateUrl: './modal-change-password.component.html',
  styleUrls: ['./modal-change-password.component.scss'],
  changeDetection: ChangeDetectionStrategy.Default
})
export class ModalChangePasswordComponent implements OnInit {
  form: FormGroup;
  userColumnNames = userColumnsConstants.labelColumns;

  resultModal = new EventEmitter<boolean>();
  
  /**
   * Отмечает ошибки по кастомной логике. 
   * В текущем виде - подсвечивает поля ошибочными до того, как пользователь их дотронется.
   */
  matcher = new PasswordConfirmationStateMatcher();

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: any,
    private dialogRef: MatDialogRef<ModalChangePasswordComponent>,
    private formBuilder: FormBuilder,
    private authService: AuthService,
    private httpService: HttpService,
    private modalService: ModalService,
    private dialog: MatDialog,
    private formBuilderService: FormBuilderService,
  ) {

  }

  ngOnInit() {
    this.generateFormForChangeUserPassword();
    console.log('this.form ', this.form)
  }

  /**
   * Генерация формы смены пароля.
   */
  generateFormForChangeUserPassword(): void {
    
    this.form = 
      this.formBuilder.group({
        password: [
          '',
          [
            Validators.required,
            Validators.pattern(regExpConstants.passwordPattern),
          ]
        ],
        confirmPassword: [
          ''
        ]
      }, { 
        validator: this.checkPasswords 
      });
  }
  
  /**
   * Проверка паролей на соответствие друг-другу.
   * @param group Группа пароля
   * @returns 
   */
  checkPasswords(group: FormGroup) {
    let pass = group.controls['password'].value;
    let confirmPass = group.controls['confirmPassword'].value;
    return pass === confirmPass ? null : { notSame: true }
  }
  
  changePassword(){
    const model: UserChangePasswordModel = {
      password: this.form.value.password
    }
    const process$ = this.httpService.changePassword(model);
    process$.subscribe({
      next: (value : any) => {
        if (value.success === true) {
          this.dialog.open(ModalComponent, {
            width: '550',
            data: {
              modalText: 'Пароль успешно изменён.'
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
