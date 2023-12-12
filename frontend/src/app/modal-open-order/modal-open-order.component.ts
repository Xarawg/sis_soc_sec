import {Component, EventEmitter, Inject, Input, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialog, MatDialogRef} from "@angular/material/dialog";
import { Order } from '../interfaces/order';
import { orderColumnsConstants } from '../constants/order.columns.constants';
import { FormBuilder, Validators, FormControl, FormGroup } from '@angular/forms';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { HttpService } from '../services/http.service';
import { OperatorProcessingOrderInputModel } from '../interfaces/operatorProcessingOrderInputModel';
import { OrderProcessingAction } from '../enums/orderProcessingAction';
import { OperatorChangeOrderInputModel } from '../interfaces/operatorChangeOrderInputModel';


@Component({
  selector: 's-modal-open-order',
  templateUrl: './modal-open-order.component.html',
  styleUrls: ['./modal-open-order.component.scss'],
})
export class ModalOpenOrderComponent implements OnInit {
  orderColumnNames = orderColumnsConstants.labelColumns;
  types: any;
  form: FormGroup;
  file_store: FileList;

  resultModal = new EventEmitter<boolean>();

  constructor(
      @Inject(MAT_DIALOG_DATA) public order: Order,
      private dialogRef: MatDialogRef<ModalOpenOrderComponent>,
      private formBuilder: FormBuilder,
      private authService: AuthService,
      private httpService: HttpService,
      private dialog: MatDialog,
      private router: Router,
  ) {
    
   }

  ngOnInit() {
    this.types = [
      {value: 0, valueView: "заявление на смену реквизитов в ПФР"}, 
      {value: 1, valueView: "запрос мер поддержки"}
    ];
    if (!!this.order && this.order.id != null) {
      this.form = this.formBuilder.group({
        id: new FormControl({value: this.order.id, disabled: true}, Validators.required),
        date: new FormControl({value: this.order.date, disabled: true}, Validators.required),
        state: new FormControl({value: this.order.state, disabled: true}, Validators.required),
        status: new FormControl({value: this.order.status, disabled: true}, Validators.required),
        snils: new FormControl({value: this.order.snils, disabled: false}, Validators.required),
        fio: [{value: this.order.fio, disabled: false}, Validators.required],
        contactData: [{value: this.order.contactData, disabled: false}, Validators.required],
        type: [{value: this.order.type, disabled: false}, Validators.required],
        body: [{value: this.order.body, disabled: true}, Validators.required],
        documents: [{value: this.order.documents, disabled: false}, Validators.required],
        supportMeasures: [{value: this.order.supportMeasures, disabled: true}, Validators.required]
      });
    } else {
      this.form = this.formBuilder.group({
        id: new FormControl({value: '', disabled: true}, Validators.required),
        date: new FormControl({value: '', disabled: true}, Validators.required),
        state: new FormControl({value: '', disabled: true}, Validators.required),
        status: new FormControl({value: '', disabled: true}, Validators.required),
        snils: new FormControl({value: '', disabled: false}, Validators.required),
        fio: ['', Validators.required],
        contactData: ['', Validators.required],
        type: ['', Validators.required],
        body: [{value: '', disabled: true}, Validators.required],
        documents: ['', Validators.required],
        supportMeasures: [{value: '', disabled: true}, Validators.required]
      });
    }
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

  /**
   * Изменение заявки, обновление данных в ней - тех, которые можно обновить.
   */
  saveOrder(): void {
    const model: OperatorChangeOrderInputModel = {
    id: this.order.id,
    snils: this.form.value.snils,
    fio: this.form.value.fio,
    contactData: this.form.value.contactData,
    type: this.form.value.type,
    supportMeasures: this.form.value.supportMeasures
  }
  this.httpService.changeOrder(model).subscribe( (data:any)=> {
    const res = data.value;
  });
  }

  declineOrder(): void {    
    const model: OperatorProcessingOrderInputModel = {
      id: this.order.id,
      action: OrderProcessingAction.Decline
    }
    this.httpService.processingOrder(model).subscribe( (data:any)=> {
      const res = data.value;
    });
  }

  sendOrder(): void {  
    const model: OperatorProcessingOrderInputModel = {
      id: this.order.id,
      action: OrderProcessingAction.Send
    }
    this.httpService.processingOrder(model).subscribe( (data:any)=> {
      const res = data.value;
    });
  }
  
  doubleOrder(): void {  
    const model: OperatorProcessingOrderInputModel = {
      id: this.order.id,
      action: OrderProcessingAction.Double
    }
    this.httpService.processingOrder(model).subscribe( (data:any)=> {
      const res = data.value;
    });
  }

  submit() {
    if (this.form.valid) {
      // console.log('form ', this.form)
    }
  }

  downloadDocscan(doc: any){
    // console.log('downloadDocscan ', doc)
  }

  cancel(){
    this.resultModal.emit(false);
    this.dialogRef.close();
  }

}
