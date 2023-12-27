import {ChangeDetectionStrategy, Component, EventEmitter, Inject, Input, OnInit} from '@angular/core';
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
  changeDetection: ChangeDetectionStrategy.Default,
  templateUrl: './modal-open-order.component.html',
  styleUrls: ['./modal-open-order.component.scss'],
})
export class ModalOpenOrderComponent implements OnInit {
  orderColumnNames = orderColumnsConstants.labelColumns;
  types: any;
  form: FormGroup;
  file_store: FileList;
  display: FormControl = new FormControl("", Validators.required);

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
      console.log('1')
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
      console.log('2')
      this.form = this.formBuilder.group({
        id: new FormControl({value: '', disabled: true}),
        date: new FormControl({value: '', disabled: true}),
        state: new FormControl({value: '', disabled: true}),
        status: new FormControl({value: '', disabled: true}),
        snils: new FormControl({value: '', disabled: false}, Validators.required),
        fio: ['', Validators.required],
        contactData: ['', Validators.required],
        type: ['', Validators.required],
        body: [{value: '', disabled: true}],
        documents: [""],
        supportMeasures: [{value: '', disabled: true}]
      });
    }
    
    this.httpService.uploadFormData$.subscribe((form:FormData | null) => {
      if (!!form) {
         this.httpService.createOrder(form)?.subscribe((res) => {},
         err => {
          console.log('HTTP Error', err)
         }
        );
       }
     })
  }
  
  /**
   * Вложить файл в заявку
   */
  handleFileInputChange(fileList: FileList | null): void {
    if (fileList != null) {
      this.file_store = fileList;
      if (fileList.length) {
        const f = fileList[0];
        const count = fileList.length > 1 ? ` и ещё ${fileList.length - 1} файл (-ов)` : "";
        this.display.patchValue(`${f.name}${count}`);
      } else {
        this.display.patchValue("");
      }
    }
  }

  /**
   * Изменение заявки, обновление данных в ней - тех, которые можно обновить.
   */
  saveOrder(): void {
    if (this.form.valid) {
      const formData = new FormData();
      formData.append("id",this.form.value.id);
      formData.append("snils",this.form.value.snils);
      formData.append("fio",this.form.value.fio);
      formData.append("contactData",this.form.value.contactData);
      formData.append("type",this.form.value.type);
      if (this.file_store != undefined) {
        for (let i = 0; i < this.file_store.length; i++) {
          formData.append("documents", this.file_store[i]);
        }
      }
      this.httpService.uploadFormData$.next(formData);
    }
  }

  /**
   * Отменить заявку - изменить её статус на отклонённую
   */
  declineOrder(): void {   
    if (this.form.valid) { 
      console.log('decline')
      const model: OperatorProcessingOrderInputModel = {
        id: this.order.id,
        action: OrderProcessingAction.Decline
      }
      this.httpService.processingOrder(model).subscribe( (data:any)=> {
        const res = data.value;
      });
    }
  }

  /**
   * Отправить заявку
   */
  sendOrder(): void {    
    if (this.form.valid) {
      console.log('send')
    const model: OperatorProcessingOrderInputModel = {
      id: this.order.id,
      action: OrderProcessingAction.Send
    }
    this.httpService.processingOrder(model).subscribe( (data:any)=> {
      const res = data.value;
    });
    }
  }
  
  /**
   * Продублировать заявку
   */
  doubleOrder(): void { 
    if (this.form.valid) { 
      console.log('double')
      const model: OperatorProcessingOrderInputModel = {
        id: this.order.id,
        action: OrderProcessingAction.Double
      }
      this.httpService.processingOrder(model).subscribe( (data:any)=> {
        const res = data.value;
      });
    }
  }

  /**
   * Скачать вложенный документ
   */
  downloadDocscan(doc: any){
    // console.log('downloadDocscan ', doc)
  }

  /**
   * Вернуться назад без изменений
   */
  cancel(){
    this.resultModal.emit(false);
    this.dialogRef.close();
  }

}
