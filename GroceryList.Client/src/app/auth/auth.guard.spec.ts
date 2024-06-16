import { TestBed } from '@angular/core/testing';
import {
  ActivatedRouteSnapshot,
  GuardResult,
  MaybeAsync,
  RouterStateSnapshot,
  UrlSegment,
  UrlTree
} from '@angular/router';
import { NgxsModule, Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { authGuard } from './auth.guard';
import { AuthState } from './ngxs-store/auth.state';
import { HttpClient, HttpHandler } from '@angular/common/http';
import { Logout } from './ngxs-store/auth.actions';

describe('AuthGuard', () => {
  let mockSelect: jasmine.Spy;
  let mockIsTokenExpired: jasmine.Spy;
  let mockDispatch: jasmine.Spy;
  const urlPath = '/dataset'

  beforeEach(async () => {
    await TestBed.configureTestingModule( {
      imports: [NgxsModule.forRoot([AuthState])],
      providers: [
        HttpClient, HttpHandler
      ]
    });
    const store = TestBed.inject(Store);
    mockSelect = spyOn(store, 'selectSnapshot');
    mockDispatch = spyOn(store, 'dispatch');
  })

  it('should return true when user is authenticated and token is not expired', async () => {
    const token = 'mockedTokenValue';
    mockSelect.withArgs(AuthState.isAuthenticated).and.returnValue(true);

    const canActivate = await runAuthGuardWithContext(getAuthGuardWithDummyUrl(urlPath));

    expect(canActivate).toBe(true);
    expect(mockDispatch).not.toHaveBeenCalled();
  });

  it('should return false and dispatch Logout when user is not authenticated', async () => {
    mockSelect.withArgs(AuthState.isAuthenticated).and.returnValue(false);

    const canActivate = await runAuthGuardWithContext(getAuthGuardWithDummyUrl(urlPath));

    expect(canActivate).toBe(false);
    expect(mockDispatch).toHaveBeenCalledWith(new Logout());
  });

  function getAuthGuardWithDummyUrl(urlPath: string): () => boolean | UrlTree | Promise<boolean | UrlTree> | Observable<boolean | UrlTree> | MaybeAsync<GuardResult>{
    const dummyRoute = new ActivatedRouteSnapshot( )
    dummyRoute.url = [ new UrlSegment(urlPath, {}) ]
    const dummyState: RouterStateSnapshot = { url: urlPath, root:  new ActivatedRouteSnapshot() }
    return () => authGuard(dummyRoute, dummyState)
  }

  async function runAuthGuardWithContext(authGuard: () => boolean | UrlTree | Promise<boolean | UrlTree> | Observable<boolean | UrlTree> | MaybeAsync<GuardResult>): Promise<boolean | UrlTree | GuardResult> {
    const result = TestBed.runInInjectionContext(authGuard)
    const authenticated = result instanceof Observable ? await handleObservableResult(result) : result;
    return authenticated
  }

  function handleObservableResult(result: Observable<boolean | UrlTree | GuardResult>): Promise<boolean | UrlTree | GuardResult> {
    return new Promise<boolean | UrlTree | GuardResult>((resolve) => {
      result.subscribe((value) => {
        resolve(value);
      });
    });
  }
})