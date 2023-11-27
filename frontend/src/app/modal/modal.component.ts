import {Component, EventEmitter, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";


@Component({
  selector: 's-modal',
  templateUrl: './modal.component.html',
  styleUrls: ['./modal.component.scss'],
})
export class ModalComponent implements OnInit {

  resultModal = new EventEmitter<boolean>();

  constructor(
      @Inject(MAT_DIALOG_DATA) public data: any,
      private dialogRef: MatDialogRef<ModalComponent>,
  ) {
    
   }

  ngOnInit() {
  }

  cancel(){
    this.resultModal.emit(false);
    this.dialogRef.close();
  }

}
