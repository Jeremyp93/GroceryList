import { inject } from '@angular/core';
import { CanDeactivateFn } from '@angular/router';
import { Observable, lastValueFrom, of } from 'rxjs';
import { ConfirmDialogService } from '../shared/confirm-dialog/confirm-dialog.service';

export interface ComponentCanDeactivate {
  canDeactivate: () => boolean | Observable<boolean>;
}

export const canDeactivateFormComponent: CanDeactivateFn<ComponentCanDeactivate> = (component: ComponentCanDeactivate) => {
    const confirmDialogService = inject(ConfirmDialogService);

    if (component.canDeactivate()) {
      return of(true);
    }
    confirmDialogService.setQuestion('WARNING: You have unsaved changes. Press Cancel to go back and save these changes, or OK to lose these changes.');
    return confirmDialogService.answer$;
}