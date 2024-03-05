import { HttpErrorResponse } from '@angular/common/http';
import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { first } from 'rxjs';
import { PreliminaryErrorDetectionStateMatcher } from 'src/app/errorStateMatcher/preliminaryErrorDetectionStateMatcher';
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

  error: any;
  loading: boolean;
  public hide: boolean = true; // Password hiding
  
  /**
   * Отмечает ошибки по кастомной логике. 
   * В текущем виде - подсвечивает поля ошибочными до того, как пользователь их дотронется.
   */
  matcher = new PreliminaryErrorDetectionStateMatcher();

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
    const model = this.loginForm.value;
    const auth$ = this.authService.login(model); 
    auth$.subscribe({
      next: (value : any) => {
        if (value.success === true) {
          const returnUrl = '/users-table';
          this.router.navigateByUrl(returnUrl);
        } else {
          this.dialog.open(ModalComponent, {
            width: '550',
            data: {
              modalText: value.error
            }
          });
        }
      },
      error: (errorValue : HttpErrorResponse) => {
        this.dialog.open(ModalComponent, {
          width: '550',
          data: {
            modalText: errorValue
          }
        });
      }
    });
  }

  register() {
    this.router.navigateByUrl('/register');
  }

  backToHome() {
    this.router.navigateByUrl('/home');
  }

}