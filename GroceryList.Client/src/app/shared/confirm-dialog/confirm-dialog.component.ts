import { Component, Renderer2, effect, inject } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ConfirmDialogService } from './confirm-dialog.service';
import { ButtonComponent } from '../button/button.component';
import { ButtonHover } from '../button/button-hover.enum';

@Component({
  selector: 'app-confirm-dialog',
  standalone: true,
  imports: [CommonModule, ButtonComponent],
  templateUrl: './confirm-dialog.component.html',
  styleUrl: './confirm-dialog.component.scss'
})
export class ConfirmDialogComponent {
  confirmDialogService = inject(ConfirmDialogService);
  #renderer = inject(Renderer2);

  get hoverChoices(): typeof ButtonHover {
    return ButtonHover;
  }

  constructor() {
    effect(() => {
      if (this.confirmDialogService.dialogQuestion()) {
        this.#renderer.addClass(document.body, 'confirm-open');
      }
    });
  }

  setAnswer = (answer: boolean) => {
    console.log('removed');
    this.#renderer.removeClass(document.body, 'confirm-open');
    this.confirmDialogService.setAnswer(answer);
  }
}
