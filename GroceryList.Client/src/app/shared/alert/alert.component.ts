import { Component, inject, ChangeDetectorRef, effect } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AlertType } from './alert.model';
import { AlertService } from './alert.service';


@Component({
  selector: 'app-alert',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './alert.component.html',
  styleUrl: './alert.component.scss'
})
export class AlertComponent {
  #alertService = inject(AlertService);
  #cd= inject(ChangeDetectorRef)
  alert = this.#alertService.alert;
  showAlert: boolean = false;

  constructor() {
    effect(() => {
      if (this.#alertService.alert().type !== AlertType.NoAlert) {
        this.showAlert = true;
        setTimeout(() => {this.showAlert = false; this.#cd.detectChanges();}, 5000)
      }
    });
  }
}
