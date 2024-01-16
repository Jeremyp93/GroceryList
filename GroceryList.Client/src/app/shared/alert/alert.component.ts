import { Component, inject, OnInit, OnDestroy, ChangeDetectorRef, computed, effect } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Subscription } from 'rxjs';
import { toObservable } from '@angular/core/rxjs-interop'

import { Alert, AlertType } from './alert.model';
import { AlertService } from './alert.service';


@Component({
  selector: 'app-alert',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './alert.component.html',
  styleUrl: './alert.component.scss'
})
export class AlertComponent implements OnInit {
  #alertService = inject(AlertService);
  #cd= inject(ChangeDetectorRef)
  alert = this.#alertService.alert;
  #alert$ = toObservable(this.#alertService.alert);
  showAlert: boolean = false;

  ngOnInit(): void {
    console.log(this.#alertService.alert().type !== AlertType.NoAlert);
    this.#alert$.subscribe(alert => {
      if (alert.type !== AlertType.NoAlert) {
        this.showAlert = true;
        setTimeout(() => {this.showAlert = false; this.#cd.detectChanges();}, 5000)
      }
    });
  }
}
