import { Injectable, signal } from "@angular/core";

import { Alert, AlertType } from "./alert.model";

@Injectable({
    providedIn: 'root'
  })
  export class AlertService {
    #alertS = signal<Alert>(new Alert(AlertType.NoAlert));
    alert = this.#alertS.asReadonly();
  
    setAlertObs(alert: Alert) {
        this.#alertS.set(alert);
    }
}