import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';

import { AuthService } from './auth.service';
import { Login } from './types/login.type';
import { Register } from './types/register.type';
import { environment } from '../../environments/environment';
import { API_ENDPOINTS } from '../constants';

describe('AuthService', () => {
  let authService: AuthService;
  let httpTestingController: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [AuthService],
    });

    authService = TestBed.inject(AuthService);
    httpTestingController = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpTestingController.verify(); // Verifies that there are no outstanding requests
  });

  it('should make a POST request for login', () => {
    const mockLogin: Login = { username: 'test@example.com', password: 'password123' };

    authService.login(mockLogin).subscribe();

    const req = httpTestingController.expectOne(`${environment.apiUrl}/${environment.userEndpoint}/${API_ENDPOINTS.LOGIN}`);
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual({ email: mockLogin.username, password: mockLogin.password });
  });

  it('should make a POST request for registration', () => {
    const mockRegister: Register = { firstName: 'John', lastName: 'Doe', email: 'example@test.ca', password: 'password' };

    authService.register(mockRegister).subscribe((response) => {
      expect(response).toBeNull();
    });

    const req = httpTestingController.expectOne(`${environment.apiUrl}/${environment.userEndpoint}`);
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual(mockRegister);

    req.flush(null);
  });
});
