import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Store } from '@ngxs/store';

import { Login, Logout } from './ngxs-store/auth.actions';
import { AuthService } from './auth.service';
import { AlertComponent } from '../shared/alert/alert.component';
import { HeaderComponent } from '../shared/header/header.component';
import { ButtonComponent } from '../shared/button/button.component';
import { Router } from '@angular/router';
import { ROUTES_PARAM } from '../constants';
import { JwtHelperService } from '@auth0/angular-jwt';
import { AuthState } from './ngxs-store/auth.state';

@Component({
  selector: 'app-auth',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, AlertComponent, HeaderComponent, ButtonComponent],
  providers: [AuthService],
  templateUrl: './auth.component.html',
  styleUrl: './auth.component.scss'
})
export class AuthComponent implements OnInit {
  ngxsStore = inject(Store);
  router = inject(Router);
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
        this.router.navigate([`/${ROUTES_PARAM.GROCERY_LIST}`])
      },
      error: (error: Error) => {
        this.errorMessage = error.message;
        this.isLoading = this.isSubmitted = false;
      }
    });
  }

  #initForm = () => {
    this.loginForm = new FormGroup({
      username: new FormControl('', [Validators.required, Validators.email]),
      password: new FormControl('', [Validators.required, Validators.minLength(6)]),
    });
  }
}
