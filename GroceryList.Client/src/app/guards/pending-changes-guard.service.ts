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
    //return component.canDeactivate() ?
    //  true :
      // NOTE: this warning message will only be shown when navigating elsewhere within your angular app;
      // when navigating away from your angular app, the browser will show a generic warning message
      // see http://stackoverflow.com/a/42207299/7307355
      //confirm('WARNING: You have unsaved changes. Press Cancel to go back and save these changes, or OK to lose these changes.');
}