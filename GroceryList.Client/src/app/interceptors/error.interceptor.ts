import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { tap } from 'rxjs';
import { inject } from '@angular/core';

import { AlertService } from '../shared/alert/alert.service';
import { Alert, AlertType } from '../shared/alert/alert.model';
import { Router } from '@angular/router';
import { Store } from '@ngxs/store';
import { Logout } from '../auth/ngxs-store/auth.actions';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const alertService = inject(AlertService);
  const store = inject(Store);

  return next(req).pipe(
    tap({error: (errorResponse: HttpErrorResponse) => {
      if (errorResponse.status === 401) {
        store.dispatch(new Logout());
        return;
      }
      let errorMessage = 'An unknown error occured.';
      if (Array.isArray(errorResponse.error)) {
        errorMessage = errorResponse.error[0];
      }
      alertService.setAlertObs(new Alert(AlertType.Error, errorMessage));
    }})
  );
}
