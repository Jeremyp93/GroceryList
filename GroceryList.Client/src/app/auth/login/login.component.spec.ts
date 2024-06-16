import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Observable } from 'rxjs';
import { Store, provideStore } from '@ngxs/store';
import { Router, provideRouter } from '@angular/router';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { provideHttpClient } from '@angular/common/http';

import { Login, Logout } from '../ngxs-store/auth.actions';
import { LOGIN_FORM } from '../../constants';
import { LoginComponent } from './login.component';
import { AuthState } from '../ngxs-store/auth.state';

describe('LoginComponent', () => {
  let component: LoginComponent;
  let fixture: ComponentFixture<LoginComponent>;
  let ngxsStore: Store;
  let router: Router;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LoginComponent],
      providers: [provideRouter([]), provideStore([AuthState]), provideHttpClient(), provideHttpClientTesting()]
    })
      .compileComponents();
    ngxsStore = TestBed.inject(Store);
    router = TestBed.inject(Router);
    fixture = TestBed.createComponent(LoginComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should have username and password properties', () => {
    expect(component.loginFormUsername).toEqual('username');
    expect(component.loginFormPassword).toEqual('password');
  });

  it('should dispatch a Logout action when the user is already logged in', () => {
    spyOn(ngxsStore, 'selectSnapshot').and.returnValue(true);
    spyOn(ngxsStore, 'dispatch');

    component.ngOnInit();

    expect(ngxsStore.dispatch).toHaveBeenCalledWith(new Logout());
  });

  it('should initialize the login form on initialization', () => {
    expect(component.loginForm).toBeDefined();
  });

  it('should dispatch a Login action with the form values when the form is valid', () => {
    spyOn(ngxsStore, 'dispatch').and.returnValue(new Observable<void>);
    component.loginForm.controls[LOGIN_FORM.USERNAME].setValue('test@example.com');
    component.loginForm.controls[LOGIN_FORM.PASSWORD].setValue('password');

    component.onSubmit();

    expect(ngxsStore.dispatch).toHaveBeenCalledWith(new Login({ username: 'test@example.com', password: 'password' }));
  });

  it('should not dispatch a Login action and should not navigate when the form is submitted with invalid data', () => {
    spyOn(ngxsStore, 'dispatch');
    spyOn(router, 'navigate');
    component.loginForm.controls[LOGIN_FORM.USERNAME].setValue('test');
    component.loginForm.controls[LOGIN_FORM.PASSWORD].setValue('');

    component.onSubmit();

    expect(ngxsStore.dispatch).not.toHaveBeenCalled();
    expect(router.navigate).not.toHaveBeenCalled();
  });

  it('should display error messages when the form is submitted with invalid data', () => {
    component.loginForm.controls[LOGIN_FORM.USERNAME].setValue('test');
    component.loginForm.controls[LOGIN_FORM.PASSWORD].setValue('');

    component.onSubmit();

    expect(component.loginForm.controls[LOGIN_FORM.USERNAME].errors).toEqual({ email: true });
    expect(component.loginForm.controls[LOGIN_FORM.PASSWORD].errors).toEqual({ required: true});
  });

  it('should not reset the loading and submission states when the form is submitted with invalid data', () => {
    component.isLoading = true;
    component.isSubmitted = true;
    component.loginForm.controls[LOGIN_FORM.USERNAME].setValue('test');
    component.loginForm.controls[LOGIN_FORM.PASSWORD].setValue('');

    component.onSubmit();

    expect(component.isLoading).toBe(true);
    expect(component.isSubmitted).toBe(true);
  });
});
