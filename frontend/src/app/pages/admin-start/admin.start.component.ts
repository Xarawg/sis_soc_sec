import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { first } from 'rxjs';
import { UserAuth } from 'src/app/interfaces/userAuth';
import { ModalComponent } from 'src/app/modal/modal.component';
import { AuthService } from 'src/app/services/auth.service';
import { FakeBackendService } from 'src/app/services/fake-backend.service';

@Component({
  selector: 'admin-start',
  templateUrl: './admin.start.component.html',
  styleUrls: ['./admin.start.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class AdminStartComponent implements OnInit {
  public loginForm: FormGroup;

  public hide: boolean = true; // Password hiding

  constructor(private backendService: FakeBackendService,
    private authService: AuthService,
    private route: ActivatedRoute,
    private router: Router,
    private dialog: MatDialog,
    private formBuilder: FormBuilder
  ) {
    // redirect to home if already logged in
    if (this.authService.userValue.value) { 
        // this.router.navigate(['/']);
    }
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
    const result = this.authService.login(auth);
    if (this.loginForm.valid && result) {
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