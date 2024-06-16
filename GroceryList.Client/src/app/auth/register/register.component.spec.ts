import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Store, provideStore } from '@ngxs/store';
import { of, throwError } from 'rxjs';
import { Router, provideRouter } from '@angular/router';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';

import { RegisterComponent } from './register.component';
import { ROUTES_PARAM } from '../../constants';
import { Register } from '../ngxs-store/auth.actions';
import { AuthState } from '../ngxs-store/auth.state';

describe('RegisterComponent', () => {
  let component: RegisterComponent;
  let fixture: ComponentFixture<RegisterComponent>;
  let ngxsStore: Store;
  let router: Router;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RegisterComponent],
      providers: [provideRouter([]), provideStore([AuthState]), provideHttpClient(), provideHttpClientTesting()]
    })
    .compileComponents();
    
    ngxsStore = TestBed.inject(Store);
    router = TestBed.inject(Router);
    fixture = TestBed.createComponent(RegisterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize form with all fields empty', () => {
    expect(component.signupForm.value).toEqual({
      firstName: '',
      lastName: '',
      email: '',
      password: '',
      confirmPassword: ''
    });
  });

  it('should submit form when all fields are filled correctly', () => {
    component.signupForm.setValue({
      firstName: 'John',
      lastName: 'Doe',
      email: 'john.doe@example.com',
      password: 'password',
      confirmPassword: 'password'
    });
    spyOn(ngxsStore, 'dispatch').and.returnValue(of());
    spyOn(router, 'navigate');
    component.onSubmit();
    expect(ngxsStore.dispatch).toHaveBeenCalledWith(new Register({
      firstName: 'John',
      lastName: 'Doe',
      email: 'john.doe@example.com',
      password: 'password',
      confirmPassword: 'password'
    } as any));
    //expect(router.navigate).toHaveBeenCalledWith([`/${ROUTES_PARAM.AUTHENTICATION.AUTH}/${ROUTES_PARAM.AUTHENTICATION.EMAIL_CONFIRM_INFO}`], { state: { email: 'john.doe@example.com' } });
  });

  it('should not submit form when all fields are filled with invalid data', () => {
    component.signupForm.setValue({
      firstName: '',
      lastName: '',
      email: 'invalidemail',
      password: 'pass',
      confirmPassword: 'pass'
    });
    spyOn(ngxsStore, 'dispatch');
    spyOn(router, 'navigate');
    component.onSubmit();
    expect(component.isLoading).toBe(false);
    expect(component.isSubmitted).toBe(true);
    expect(ngxsStore.dispatch).not.toHaveBeenCalled();
    expect(router.navigate).not.toHaveBeenCalled();
  });

  it('should not submit form multiple times when all fields are filled with invalid data', () => {
    component.signupForm.setValue({
      firstName: '',
      lastName: '',
      email: 'invalidemail',
      password: 'pass',
      confirmPassword: 'pass'
    });
    spyOn(ngxsStore, 'dispatch');
    spyOn(router, 'navigate');
    component.onSubmit();
    component.onSubmit();
    component.onSubmit();
    expect(component.isLoading).toBe(false);
    expect(component.isSubmitted).toBe(true);
    expect(ngxsStore.dispatch).not.toHaveBeenCalled();
    expect(router.navigate).not.toHaveBeenCalled();
  });

  it('should handle server error when submitting form', () => {
    component.signupForm.setValue({
      firstName: 'John',
      lastName: 'Doe',
      email: 'john.doe@example.com',
      password: 'password',
      confirmPassword: 'password'
    });
  
    spyOn(ngxsStore, 'dispatch').and.returnValue(throwError(() => new Error('error')));
    spyOn(router, 'navigate');
    component.onSubmit();
    expect(component.isLoading).toBe(false);
    expect(component.isSubmitted).toBe(false);
    expect(ngxsStore.dispatch).toHaveBeenCalled();
    expect(router.navigate).not.toHaveBeenCalled();
  });
});
