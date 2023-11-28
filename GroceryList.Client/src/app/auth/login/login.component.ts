import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Store } from '@ngxs/store';
import { Router, RouterModule } from '@angular/router';

import { Login, Logout } from '../ngxs-store/auth.actions';
import { AlertComponent } from '../../shared/alert/alert.component';
import { HeaderComponent } from '../../shared/header/header.component';
import { ButtonComponent } from '../../shared/button/button.component';
import { LOGIN_FORM, ROUTES_PARAM } from '../../constants';
import { AuthState } from '../ngxs-store/auth.state';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, AlertComponent, HeaderComponent, ButtonComponent, RouterModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent implements OnInit {
  ngxsStore = inject(Store);
  router = inject(Router);
  readonly registerRoute: string = '/' + ROUTES_PARAM.REGISTER;
  readonly loginFormUsername: string = LOGIN_FORM.USERNAME;
  readonly loginFormPassword: string = LOGIN_FORM.PASSWORD;
  loginForm!: FormGroup;
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
    if (this.loginForm.invalid) return;
    this.ngxsStore.dispatch(new Login(this.loginForm.value)).subscribe({
      next: () => {
        this.isLoading = this.isSubmitted = false;
        this.router.navigate([`/${ROUTES_PARAM.GROCERY_LIST}`]);
      },
      error: (error: Error) => {
        this.errorMessage = error.message;
        this.isLoading = this.isSubmitted = false;
      }
    });
  }

  #initForm = () => {
    this.loginForm = new FormGroup({
      [LOGIN_FORM.USERNAME]: new FormControl('', [Validators.required, Validators.email]),
      [LOGIN_FORM.PASSWORD]: new FormControl('', [Validators.required, Validators.minLength(6)]),
    });
  }
}