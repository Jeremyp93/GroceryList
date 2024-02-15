import { Component, OnInit, inject } from '@angular/core';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { RESET_PASSWORD_FORM, ROUTES_PARAM, SIGNUP_FORM } from '../../constants';
import { AuthService } from '../auth.service';
import { CommonModule } from '@angular/common';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ButtonComponent } from '../../shared/button/button.component';
import { HeaderComponent } from '../../shared/header/header.component';
import { InputFieldComponent } from '../../shared/input-field/input-field.component';
import { InputType } from '../../shared/input-field/input-type.enum';
import { passwordMatchValidator } from '../validators/password.validator';

@Component({
  selector: 'app-reset-password',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, HeaderComponent, ButtonComponent, RouterModule, InputFieldComponent],
  templateUrl: './reset-password.component.html',
  styleUrl: './reset-password.component.scss'
})
export class ResetPasswordComponent implements OnInit {
  #route = inject(ActivatedRoute);
  #router = inject(Router);
  #authService = inject(AuthService);

  readonly loginRoute: string = '/' + ROUTES_PARAM.AUTHENTICATION.LOGIN;
  readonly signupFormPassword: string = RESET_PASSWORD_FORM.PASSWORD;
  readonly signupFormConfirmPassword: string = RESET_PASSWORD_FORM.CONFIRM_PASSWORD;
  resetForm!: FormGroup;
  isLoading: boolean = false;
  isSubmitted: boolean = false;
  #email: string = '';
  #token: string = '';

  get inputTypes(): typeof InputType {
    return InputType;
  }

  ngOnInit(): void {
    this.#route.queryParams.subscribe(params => {
      const token = params['token'];
      const email = params['email'];
      if (token && email) {
        this.#email = email;
        this.#token = token;
      } else {
        this.#router.navigate(['/error']);
      }
    });
    this.#initForm();
  }

  onSubmit = () => {
    this.isSubmitted = true;
    if (this.resetForm.invalid) return;
    this.isLoading = true;
    this.#authService.resetPassword({email: this.#email, token: this.#token, password: this.resetForm.value[RESET_PASSWORD_FORM.PASSWORD]}).subscribe({
      next: () => {
        this.#router.navigate([`/${ROUTES_PARAM.AUTHENTICATION.LOGIN}`], { queryParams: { reset: true } });
      },
      error: () => {
        this.#router.navigate(['/error']);
      }
    });
  }

  #initForm = () => {
    this.resetForm = new FormGroup({
      [RESET_PASSWORD_FORM.PASSWORD]: new FormControl('', [Validators.required, Validators.minLength(6)]),
      [RESET_PASSWORD_FORM.CONFIRM_PASSWORD]: new FormControl('', [Validators.required, passwordMatchValidator(SIGNUP_FORM.PASSWORD)])
    });
  }
}
