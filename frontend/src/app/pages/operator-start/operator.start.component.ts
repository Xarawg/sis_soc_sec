import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { first } from 'rxjs';
import { UserAuth } from 'src/app/interfaces/userAuth';
import { ModalComponent } from 'src/app/modal/modal.component';
import { AuthService } from 'src/app/services/auth.service';
import { FakeBackendService } from 'src/app/services/fake-backend.service';

@Component({
  selector: 'operator-start',
  templateUrl: './operator.start.component.html',
  styleUrls: ['./operator.start.component.scss']
})
export class OperatorStartComponent implements OnInit {

  form: FormGroup;
  error: any;
  loading: boolean;

  constructor(private backendService: FakeBackendService,
    private authService: AuthService,
    private route: ActivatedRoute,
    private router: Router,
    private dialog: MatDialog,
    private formBuilder: FormBuilder
  ) {

  }
  ngOnInit(): void {
    this.form = this.formBuilder.group({
      userName: ['', [Validators.required]],
      password: ['', [Validators.required]],
    });
  }

  submit() {
    const auth: UserAuth = {
      userName: this.form.value.userName,
      password: this.form.value.password
    }
    const result = this.authService.login(auth)
    .pipe(first())
    .subscribe({
        next: () => {
            // get return url from route parameters or default to '/'
            const returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
            this.router.navigate([returnUrl]);
        },
        error: error => {
            this.error = error;
            this.loading = false;
        }
    });;
    if (this.form.valid) {
      this.router.navigateByUrl('/orders-table');
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

  // submit() {
  //   if (this.form.valid) {
  //     const result = this.authService.login(this.form.value);
  //     if (result === true) this.router.navigateByUrl('/orders-table');
  //     else {
  //       this.dialog.open(ModalComponent, {
  //         width: '550',
  //         data: {
  //           modalText: 'Данные введены некорректно.'
  //         }
  //       });
  //     }
  //   }
  // }

  register() {
    this.router.navigateByUrl('/operator-register');
  }
}
