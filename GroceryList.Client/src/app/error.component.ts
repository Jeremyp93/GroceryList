// error.component.ts
import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-error',
  standalone: true,
  template: `
    <div class="error-container">
      <h2>{{ title }}</h2>
      <p>{{ message }}</p>
    </div>
  `,
  styles: [
    `
      .error-container {
        text-align: center;
        margin-top: 50px;
      }

      h2 {
        color: red;
      }
    `,
  ],
})
export class ErrorComponent {
  @Input() title: string = 'Error';
  @Input() message: string = 'An unexpected error occurred.';
}