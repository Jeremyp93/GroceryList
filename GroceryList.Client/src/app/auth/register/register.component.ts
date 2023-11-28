import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AbstractControl, FormControl, FormGroup, ReactiveFormsModule, ValidatorFn, Validators } from '@angular/forms';
import { Store } from '@ngxs/store';
import { Router, RouterModule } from '@angular/router';

import { Login, Logout, Register } from '../ngxs-store/auth.actions';
import { AuthService } from '../auth.service';
import { AlertComponent } from '../../shared/alert/alert.component';
import { HeaderComponent } from '../../shared/header/header.component';
import { ButtonComponent } from '../../shared/button/button.component';
import { SIGNUP_FORM, ROUTES_PARAM } from '../../constants';
import { AuthState } from '../ngxs-store/auth.state';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, AlertComponent, HeaderComponent, ButtonComponent, RouterModule],
  providers: [AuthService],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent implements OnInit {
  ngxsStore = inject(Store);
  router = inject(Router);
  readonly loginRoute: string = '/' + ROUTES_PARAM.LOGIN;
  readonly signupFormFirstName: string = SIGNUP_FORM.FIRST_NAME;
  readonly signupFormLastName: string = SIGNUP_FORM.LAST_NAME;
  readonly signupFormEmail: string = SIGNUP_FORM.EMAIL;
  readonly signupFormPassword: string = SIGNUP_FORM.PASSWORD;
  readonly signupFormConfirmPassword: string = SIGNUP_FORM.CONFIRM_PASSWORD;
  signupForm!: FormGroup;
  errorMessage: string = '';
  isLoading: boolean = false;
  isSubmitted: boolean = false;

  ngOnInit(): void {
    const isLoggedIn = this.ngxsStore.selectSnapshot(AuthState.isAuthenticated);
    if (isLoggedIn) {
      this.ngxsStore.dispatch(new Logout());
    }
    this.#initForm();
  }

  onSubmit = () => {
    this.errorMessage = '';
    this.isSubmitted = true;
    this.isLoading = true;
    if (this.signupForm.invalid) return;
    this.ngxsStore.dispatch(new Register(this.signupForm.value)).subscribe({
      next: () => {
        this.isLoading = this.isSubmitted = false;
        this.router.navigate([`/${ROUTES_PARAM.LOGIN}`]);
      },
      error: (error: Error) => {
        this.errorMessage = error.message;
        this.isLoading = this.isSubmitted = false;
      }
    });
  }

  #initForm = () => {
    this.signupForm = new FormGroup({
      [SIGNUP_FORM.FIRST_NAME]: new FormControl('', [Validators.required]),
      [SIGNUP_FORM.LAST_NAME]: new FormControl('', [Validators.required]),
      [SIGNUP_FORM.EMAIL]: new FormControl('', [Validators.required, Validators.email]),
      [SIGNUP_FORM.PASSWORD]: new FormControl('', [Validators.required, Validators.minLength(6)]),
      [SIGNUP_FORM.CONFIRM_PASSWORD]: new FormControl('', [Validators.required, this.#passwordMatchValidator(SIGNUP_FORM.PASSWORD)])
    });
  }

  #passwordMatchValidator = (otherControlName: string): ValidatorFn => {
    return (control: AbstractControl): { [key: string]: any } | null => {
      const otherControl = control.root.get(otherControlName);

      if (otherControl && control.value !== otherControl.value) {
        return { passwordMismatch: true };
      }

      return null;
    };
  }
}
