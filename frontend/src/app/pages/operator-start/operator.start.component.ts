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

  loginForm: FormGroup;
  error: any;
  loading: boolean;
  public hide: boolean = true; // Password hiding

  constructor(private backendService: FakeBackendService,
    private authService: AuthService,
    private route: ActivatedRoute,
    private router: Router,
    private dialog: MatDialog,
    private formBuilder: FormBuilder
  ) {

  }
  ngOnInit(): void {
    this.loginForm = this.formBuilder.group({
      userName: ['', [Validators.required]],
      password: ['', [Validators.required]],
    });
  }

  public onLogin(): void {
    this.markAsDirty(this.loginForm);
    const auth: UserAuth = {
      userName: this.loginForm.value.userName,
      password: this.loginForm.value.password
    }
    const result = this.authService.login(auth)
    .pipe(first())
    .subscribe({
        next: () => {
            // get return url from route parameters or default to '/'
            // const returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
            // console.log('navigate return /')
            // this.router.navigate([returnUrl]);
        },
        error: error => {
            this.error = error;
            this.loading = false;
        }
    });;
    if (this.loginForm.valid) {
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

  private markAsDirty(group: FormGroup): void {
    group.markAsDirty();
    // tslint:disable-next-line:forin
    for (const i in group.controls) {
      group.controls[i].markAsDirty();
    }
  }

  register() {
    this.router.navigateByUrl('/register');
  }

  backToHome() {
    this.router.navigateByUrl('/home');
  }

}