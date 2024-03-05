import { ChangeDetectionStrategy, Component, EventEmitter, Inject, Input, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialog, MatDialogRef } from "@angular/material/dialog";
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
import { PreliminaryErrorDetectionStateMatcher } from '../errorStateMatcher/preliminaryErrorDetectionStateMatcher';
import { Docscan } from '../interfaces/docscan';
import { OperatorGetDocscanModel } from '../interfaces/operatorGetDocscanModel';
import { saveAs } from '@progress/kendo-file-saver'
import { ModalService } from '../services/modal.service';
import { ModalComponent } from '../modal/modal.component';
import { FormBuilderService } from '../services/form.builder.service';

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
  orderType: any;

  /**
   * 
   */
  resultModal = new EventEmitter<boolean>();


  /**
   * Отмечает ошибки по кастомной логике. 
   * В текущем виде - подсвечивает поля ошибочными до того, как пользователь их дотронется.
   */
  matcher = new PreliminaryErrorDetectionStateMatcher();

  constructor(
    @Inject(MAT_DIALOG_DATA) public order: Order,
    private dialogRef: MatDialogRef<ModalOpenOrderComponent>,
    private formBuilder: FormBuilder,
    private authService: AuthService,
    private httpService: HttpService,
    private modalService: ModalService,
    private dialog: MatDialog,
    private router: Router,
    private formBuilderService: FormBuilderService
  ) {

  }

  ngOnInit() {
    this.types = [
      { value: 0, valueView: "заявление на смену реквизитов в ПФР" },
      { value: 1, valueView: "запрос мер поддержки" }
    ];
    this.orderType = this.types?.filter((x: any) => x?.value == this.order?.type)[0]?.valueView;

    if (!!this.order && this.order.id != null) {
      this.form = this.formBuilder.group({
        id: new FormControl({ value: this.order.id, disabled: true }, Validators.required),
        date: new FormControl({ value: this.order.date, disabled: true }, Validators.required),
        state: new FormControl({ value: this.order.state, disabled: true }, Validators.required),
        status: new FormControl({ value: this.order.status, disabled: true }, Validators.required),
        snils: new FormControl({ value: this.order.snils, disabled: false }, Validators.required),
        fio: [{ value: this.order.fio, disabled: false }, Validators.required],
        contactData: [{ value: this.order.contactData, disabled: false }, Validators.required],
        type: [{ value: this.order.type, disabled: false }, Validators.required],
        body: [{ value: this.order.body, disabled: true }, Validators.required],
        documents: [{ value: this.order.documents, disabled: false }, Validators.required],
        supportMeasures: [{ value: this.order.supportMeasures, disabled: true }, Validators.required]
      });
    } else {
      this.form = this.formBuilderService.generateFormForCreatingNewOrder();
    }
    
    this.httpService.uploadFormData$.subscribe((form: FormData | null) => {
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
   * Сохранение заявки, обновление данных в ней - тех, которые можно обновить.
   */
  saveOrder(): void {
    if (this.form.valid) {
      const formData = new FormData();
      formData.append("id", this.form.value.id);
      formData.append("snils", this.form.value.snils);
      formData.append("fio", this.form.value.fio);
      formData.append("contactData", this.form.value.contactData);
      formData.append("type", this.form.value.type);
      if (this.file_store != undefined) {
        for (let i = 0; i < this.file_store.length; i++) {
          formData.append("documents", this.file_store[i]);
        }
      }
      this.httpService.uploadFormData$.next(formData);

      this.createOrder(formData);

      // Сообщаем таблице, что данные обновились.
      this.modalService.changed.next(null);

      // Закрываем модальное окно, т.к. команда выполнена успешно.
      this.dialogRef.close();
    } else {
      this.dialog.open(ModalComponent, {
        width: '550',
        data: {
          modalText: 'Заполните все поля формы.'
        }
      });
    }
  }

  createOrder(form: FormData): void {
    if (!!form) {
      const order$ = this.httpService.createOrder(form);
      order$.subscribe({
        next: (value : any) => {
          if (value.success === true) {
            // Сообщаем таблице, что данные обновились.
            this.modalService.changed.next(value);

            this.dialog.open(ModalComponent, {
              width: '550',
              data: {
                modalText: 'Заявка создана успешно.'
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
  }

  /**
   * Изменение заявки, обновление данных в ней - тех, которые можно обновить.
   */
  changeOrder(id: any): void {
    if (this.form.valid) {
      const model: OperatorChangeOrderInputModel = {
        id: id,
        snils: this.form.value.snils,
        fio: this.form.value.fio,
        contactData: this.form.value.contactData,
        type: this.form.value.type,
        supportMeasures: this.form.value.supportMeasures
      }

      const order$ = this.httpService.changeOrder(model);
      order$.subscribe({
        next: (value : any) => {
          if (value.success === true) {
            // Сообщаем таблице, что данные обновились.
            this.modalService.changed.next(value);

            this.dialog.open(ModalComponent, {
              width: '550',
              data: {
                modalText: 'Заявка изменена успешно.'
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
   * Отменить заявку - изменить её статус на отклонённую
   */
  declineOrder(): void {
    const model: OperatorProcessingOrderInputModel = {
      id: this.order.id,
      action: OrderProcessingAction.Decline
    }

    const order$ = this.httpService.processingOrder(model);
    order$.subscribe({
      next: (value : any) => {
        if (value.success === true) {
          // Сообщаем таблице, что данные обновились.
          this.modalService.changed.next(value);

          this.dialog.open(ModalComponent, {
            width: '550',
            data: {
              modalText: 'Заявка отменена успешно.'
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

  /**
   * Отправить заявку
   */
  sendOrder(): void {
    const model: OperatorProcessingOrderInputModel = {
      id: this.order.id,
      action: OrderProcessingAction.Send
    }

    const order$ = this.httpService.processingOrder(model);
    order$.subscribe({
      next: (value : any) => {
        if (value.success === true) {
          // Сообщаем таблице, что данные обновились.
          this.modalService.changed.next(value);

          this.dialog.open(ModalComponent, {
            width: '550',
            data: {
              modalText: 'Заявка отправлена успешно.'
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

  /**
   * Продублировать заявку
   */
  doubleOrder(): void {
    const model: OperatorProcessingOrderInputModel = {
      id: this.order.id,
      action: OrderProcessingAction.Double
    }

    const order$ = this.httpService.processingOrder(model);
    order$.subscribe({
      next: (value : any) => {
        if (value.success === true) {
          // Сообщаем таблице, что данные обновились.
          this.modalService.changed.next(value);

          this.dialog.open(ModalComponent, {
            width: '550',
            data: {
              modalText: 'Заявка продублирована успешно.'
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

  /**
   * Скачать вложенный документ
   */
  downloadDocscan(doc: any) {
    const model: OperatorGetDocscanModel = { 'IdDoc': doc.id };
    const change$ = this.httpService.getOrderFileByIdDoc(model);
    change$.subscribe({
      next: (value : any) => {
        if (!!value) {
          // Сохраняем файл на устройство пользователя.
          saveAs(value, doc.fileName);
        } else {
          this.dialog.open(ModalComponent, {
            width: '550',
            data: {
              modalText: 'Ошибка скачивания файла.'
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

  /**
   * Вернуться назад без изменений
   */
  cancel() {
    this.resultModal.emit(false);
    this.dialogRef.close();
  }

}