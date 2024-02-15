import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';

import { ButtonComponent } from '../../../shared/button/button.component';
import { HeaderComponent } from '../../../shared/header/header.component';
import { InputFieldComponent } from '../../../shared/input-field/input-field.component';
import { FORGOT_PASSWORD_FORM, ROUTES_PARAM } from '../../../constants';
import { InputType } from '../../../shared/input-field/input-type.enum';
import { AuthService } from '../../auth.service';

@Component({
  selector: 'app-forgot-password',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, HeaderComponent, ButtonComponent, RouterModule, InputFieldComponent],
  templateUrl: './forgot-password.component.html',
  styleUrl: './forgot-password.component.scss'
})
export class ForgotPasswordComponent implements OnInit {
  #authService = inject(AuthService);
  #router = inject(Router);

  readonly loginRoute: string = `/${ROUTES_PARAM.AUTHENTICATION.AUTH}/${ROUTES_PARAM.AUTHENTICATION.LOGIN}`;
  readonly emailField: string = FORGOT_PASSWORD_FORM.EMAIL;
  forgotForm!: FormGroup;
  isLoading = false;
  isSubmitted = false;
  showMessage = false;

  get inputTypes(): typeof InputType {
    return InputType;
  }

  ngOnInit(): void {
    console.log('teeest');
    this.#initForm();
  }

  onSubmit = () => {
    this.isSubmitted = true;
    if (this.forgotForm.invalid) return;
    this.isLoading = true;
    this.#authService.forgotPassword(this.forgotForm.value[FORGOT_PASSWORD_FORM.EMAIL]).subscribe({
      next: () => {
        this.isLoading = this.isSubmitted = false;
        this.showMessage = true;
      },
      error: () => {
        this.isLoading = this.isSubmitted = false;
      }
    });
  }

  #initForm = () => {
    this.forgotForm = new FormGroup({
      [FORGOT_PASSWORD_FORM.EMAIL]: new FormControl('', [Validators.required, Validators.email]),
    });
  }
}
