import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { UserAuth } from 'src/app/interfaces/userAuth';
import { ModalComponent } from 'src/app/modal/modal.component';
import { FakeBackendService } from 'src/app/services/fake-backend.service';

@Component({
  selector: 'admin-start',
  templateUrl: './admin.start.component.html',
  styleUrls: ['./admin.start.component.scss']
})
export class AdminStartComponent implements OnInit {

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
    const auth: UserAuth = {
      login: this.form.value.login,
      password: this.form.value.password
    }
    const result = this.backendService.loginAdmin(auth);
    if (this.form.valid && result) {
      this.router.navigateByUrl('/users-table');
    }
    else {
      this.dialog.open(ModalComponent, {
        width: '550',
        data: {
          modalText: 'Данные введены некорректно.'
        }
      });
    }
  }

  register() {
    this.router.navigateByUrl('/admin-register');
  }
}
