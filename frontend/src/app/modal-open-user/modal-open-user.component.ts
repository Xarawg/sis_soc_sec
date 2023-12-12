import { Component, EventEmitter, Inject, Input, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { Order } from '../interfaces/order';
import { userColumnsConstants } from '../constants/user.columns.constants';
import { FormBuilder, Validators, FormControl, FormGroup } from '@angular/forms';
import { User } from '../interfaces/user';
import { AuthService } from '../services/auth.service';


@Component({
  selector: 's-modal-open-user',
  templateUrl: './modal-open-user.component.html',
  styleUrls: ['./modal-open-user.component.scss'],
})
export class ModalOpenUserComponent implements OnInit {
  userColumnNames = userColumnsConstants.labelColumns;
  states = userColumnsConstants.states;

  form: FormGroup;
  file_store: FileList;
  statuses:any;
  roles:any;
  statusPlaceholderId:any;

  resultModal = new EventEmitter<boolean>();

  constructor(
    @Inject(MAT_DIALOG_DATA) public user: User,
    private dialogRef: MatDialogRef<ModalOpenUserComponent>,
    private formBuilder: FormBuilder,
    private authService: AuthService
  ) {

  }

  ngOnInit() {    
    this.statuses = [
      {value: -2, valueView: "Заблокированный"}, 
      {value: -1, valueView: "Отклоненный"},
      {value: 0, valueView: "Вновь заведенный пользователь"}, 
      {value: 1, valueView: "Зарегистрированный пользователь"}
    ];
    this.getStatusPlaceholder();
    this.roles = this.getAvailableRolesForChange();
    if (!!this.user) {
      this.form = this.formBuilder.group({
        userName: new FormControl({value: this.user.userName, disabled: true}, Validators.required),
        userRole: [this.user.userRole, Validators.required],
        fio: [this.user.fio, Validators.required],
        organization: [this.user.organization, Validators.required],
        inn: [this.user.inn, Validators.required],
        address: [this.user.address, Validators.required],
        email: [this.user.email, Validators.required],
        phoneNumber: [this.user.phoneNumber, Validators.required],
        status: [this.user.status, Validators.required]
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
        status: ['', Validators.required],
        password: ['', Validators.required]
      });
    }
  }

  getStatusPlaceholder(): void {
    const res = this.statuses.filter((x:any) => x.valueView === this.user.status);
    console.log('res ', res)
    this.statusPlaceholderId =  res != null ? res : 0;
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
    switch(this.authService.userValue.value['role']) {
      case 3: 
        return [
                {value: 0, valueView: "Отсутствует"}, 
                {value: 1, valueView: "Оператор"},
                {value: 2, valueView: "Администратор"}
              ];
      default:
        return [
                {value: 0, valueView: "Отсутствует"}, 
                {value: 1, valueView: "Оператор"},
              ];
    }
  }

  declineUser(): void {
    console.log('declineUser')
  }

  registerUser(): void {
    console.log('registerUser')
  }

  saveUser(): void {
    console.log('saveUser')
  }
     
  blockUser(): void {
    console.log('blockUser')
  }

  submit() {
    if (this.form.valid) {
      console.log('form ', this.form)
    }
  }

  cancel() {
    this.resultModal.emit(false);
    this.dialogRef.close();
  }

}
