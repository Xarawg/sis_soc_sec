import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { first } from 'rxjs';
import { PreliminaryErrorDetectionStateMatcher } from 'src/app/errorStateMatcher/preliminaryErrorDetectionStateMatcher';
import { UserAuth } from 'src/app/interfaces/userAuth';
import { ModalComponent } from 'src/app/modal/modal.component';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'operator-start',
  templateUrl: './operator.start.component.html',
  styleUrls: ['./operator.start.component.scss']
})
export class OperatorStartComponent implements OnInit {

  loginForm: FormGroup;
  error: any;
  loading: boolean;
  
  /**
   * Отмечает ошибки по кастомной логике. 
   * В текущем виде - подсвечивает поля ошибочными до того, как пользователь их дотронется.
   */
  matcher = new PreliminaryErrorDetectionStateMatcher();

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
    const model = this.loginForm.value;
    const auth$ = this.authService.login(model); 
    auth$.subscribe({
      next: (value : any) => {
        if (value.success === true) {
          const returnUrl = '/orders-table';
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

  register() {
    this.router.navigateByUrl('/register');
  }

  backToHome() {
    this.router.navigateByUrl('/home');
  }

}