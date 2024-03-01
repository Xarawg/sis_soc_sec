import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { first } from 'rxjs';
import { MyErrorStateMatcher } from 'src/app/errorStateMatcher/errorStateMatcher';
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
  matcher = new MyErrorStateMatcher();

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
      .subscribe(
        (result) => {
          const returnUrl = '/users-table';
          this.router.navigateByUrl(returnUrl);
        },
        (error) => {
          this.dialog.open(ModalComponent, {
            width: '550',
            data: {
              modalText: 'Данные введены некорректно.'
            }
          });
        }
      );
  }

  register() {
    this.router.navigateByUrl('/register');
  }

  backToHome() {
    this.router.navigateByUrl('/home');
  }

}