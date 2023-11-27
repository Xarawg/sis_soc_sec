import {Component, EventEmitter, Inject, Input, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import { Order } from '../interfaces/order';
import { orderColumnsConstants } from '../constants/order.columns.constants';
import { FormBuilder, Validators, FormControl, FormGroup } from '@angular/forms';


@Component({
  selector: 's-modal-open-order',
  templateUrl: './modal-open-order.component.html',
  styleUrls: ['./modal-open-order.component.scss'],
})
export class ModalOpenOrderComponent implements OnInit {
  orderColumnNames = orderColumnsConstants.labelColumns;
  states = orderColumnsConstants.states;
  
  form: FormGroup;
  file_store: FileList;

  resultModal = new EventEmitter<boolean>();

  constructor(
      @Inject(MAT_DIALOG_DATA) public order: Order,
      private dialogRef: MatDialogRef<ModalOpenOrderComponent>,
      private formBuilder: FormBuilder
  ) {
    
   }

  ngOnInit() {
    this.form = this.formBuilder.group({
      idOrder: new FormControl({value: '', disabled: true}, Validators.required),
      date: new FormControl({value: '', disabled: true}, Validators.required),
      state: new FormControl({value: '', disabled: true}, Validators.required),
      status: new FormControl({value: '', disabled: true}, Validators.required),
      snils: new FormControl({value: '', disabled: true}, Validators.required),
      fio: ['', Validators.required],
      contactData: ['', Validators.required],
      type: ['', Validators.required],
      body: ['', Validators.required],
      documents: ['', Validators.required],
      supportMeasures: ['', Validators.required]
    });
  }

  getState(state: any): string {
    const result: number = +state +3;
    return this.states[result];
  }

  
  handleFileInputChange(fileList: FileList | null): void {
    if (fileList != null) {
      this.file_store = fileList;
      if (fileList.length) {
        const f = fileList[0];
        const count = fileList.length > 1 ? `(+${fileList.length - 1} files)` : "";
        (this.form.get('documents') as FormControl).patchValue(`${f.name}${count}`);
      } else {
        (this.form.get('documents') as FormControl).patchValue("");
      }
    }
  }

  saveOrder(): void {
    console.log('saveOrder')
  }

  declineOrder(): void {
    console.log('declineOrder')
  }

  sendOrder(): void {
    console.log('sendOrder')
  }
  
  doubleOrder(): void {
    console.log('doubleOrder')
  }

  submit() {
    if (this.form.valid) {
      console.log('form ', this.form)
    }
  }

  cancel(){
    this.resultModal.emit(false);
    this.dialogRef.close();
  }

}
