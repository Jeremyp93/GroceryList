import { Component, inject } from '@angular/core';
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

  get hoverChoices(): typeof ButtonHover {
    return ButtonHover;
  }

  setAnswer = (answer: boolean) => {
    this.confirmDialogService.setAnswer(answer);
  }
}
