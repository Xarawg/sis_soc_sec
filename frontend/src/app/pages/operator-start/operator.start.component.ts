import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { ModalComponent } from 'src/app/modal/modal.component';
import { FakeBackendService } from 'src/app/services/fake-backend.service';

@Component({
  selector: 'operator-start',
  templateUrl: './operator.start.component.html',
  styleUrls: ['./operator.start.component.scss']
})
export class OperatorStartComponent implements OnInit {

  form: FormGroup;

  constructor(private backendService: FakeBackendService,
    private router: Router,
    private dialog: MatDialog,
    private formBuilder: FormBuilder
  ) {

  }
  ngOnInit(): void {
    this.form = this.formBuilder.group({
      login: ['', [Validators.required]],
      password: ['', [Validators.required]],
    });
  }

  submit() {
    if (this.form.valid) {
      const result = this.backendService.loginOperator(this.form.value);
      if (result === true) this.router.navigateByUrl('/order-table');
      else {
        this.dialog.open(ModalComponent, {
          width: '550',
          data: {
            modalText: 'Данные введены некорректно.'
          }
        });
      }
    }
  }

  register() {
    this.router.navigateByUrl('/operator-register');
  }
}
