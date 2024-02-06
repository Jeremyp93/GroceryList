import { Component, OnInit, inject } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { AuthService } from '../auth.service';
import { ROUTES_PARAM } from '../../constants';

@Component({
  selector: 'app-callback-confirm-email',
  standalone: true,
  imports: [],
  template: '<p>Redirecting...</p>',
})
export class ConfirmEmailCallbackComponent implements OnInit {
  #route = inject(ActivatedRoute);
  #router = inject(Router);
  #authService = inject(AuthService);

  ngOnInit(): void {
    this.#route.queryParams.subscribe(params => {
      const token = params['token'];
      const email = params['email'];
      if (token && email) {
        this.#authService.confirmEmail(token, email).subscribe({
          next: () => {
            this.#router.navigate([`/${ROUTES_PARAM.AUTHENTICATION.LOGIN}`], { queryParams: { confirmed: true } });
          },
          error: () => {
            this.#router.navigate(['/error']);
          }
        });
      } else {
        this.#router.navigate(['/error']);
      }
    });
  }
}
