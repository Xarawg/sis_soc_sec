import { Component, EventEmitter, Inject, Input, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { Order } from '../interfaces/order';
import { userColumnsConstants } from '../constants/user.columns.constants';
import { FormBuilder, Validators, FormControl, FormGroup } from '@angular/forms';
import { User } from '../interfaces/user';


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

  resultModal = new EventEmitter<boolean>();

  constructor(
    @Inject(MAT_DIALOG_DATA) public user: User,
    private dialogRef: MatDialogRef<ModalOpenUserComponent>,
    private formBuilder: FormBuilder
  ) {

  }

  ngOnInit() {
    if (!!this.user) {
      this.form = this.formBuilder.group({
        login: new FormControl({value: '', disabled: true}, Validators.required),
        role: ['', Validators.required],
        fio: ['', Validators.required],
        organization: ['', Validators.required],
        innOrganization: ['', Validators.required],
        addressOrganization: ['', Validators.required],
        email: ['', Validators.required],
        phone: ['', Validators.required],
        state: ['', Validators.required]
      });
    } else {
      this.form = this.formBuilder.group({
        login: ['', Validators.required],
        role: ['', Validators.required],
        fio: ['', Validators.required],
        organization: ['', Validators.required],
        innOrganization: ['', Validators.required],
        addressOrganization: ['', Validators.required],
        email: ['', Validators.required],
        phone: ['', Validators.required],
        state: ['', Validators.required]
      });
    }
  }

  /** Получаем строку со значением состояния */
  getState(state: any): string {
    //самое младшее значение в enum имеет значение -2, а данные в массиве states начинаются с 0. Поэтому прибавляем 2.
    const result: number = +state + 2; 
    return this.states[result];
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
