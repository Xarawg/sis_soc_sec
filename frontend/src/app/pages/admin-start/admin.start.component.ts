import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { first } from 'rxjs';
import { UserAuth } from 'src/app/interfaces/userAuth';
import { ModalComponent } from 'src/app/modal/modal.component';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'admin-start',
  templateUrl: './admin.start.component.html',
  styleUrls: ['./admin.start.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class AdminStartComponent implements OnInit {
  public loginForm: FormGroup;

  public hide: boolean = true; // Password hiding

  constructor(
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
    const auth = this.loginForm.value;
    
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
            // this.error = error;
            // this.loading = false;
        }
    });;
    if (this.loginForm.valid && result) {
      const returnUrl = '/users-table';
      this.router.navigateByUrl(returnUrl);
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

  // private markAsDirty(group: FormGroup): void {
  //   group.markAsDirty();
  //   for (const i in group.controls) {
  //     group.controls[i].markAsDirty();
  //   }
  // }

  register() {
    this.router.navigateByUrl('/register');
  }

  backToHome() {
    this.router.navigateByUrl('/home');
  }

}