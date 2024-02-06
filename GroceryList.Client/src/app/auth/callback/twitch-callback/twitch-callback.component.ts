import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';

import { ROUTES_PARAM } from '../../../constants';
import { Store } from '@ngxs/store';
import { CallbackTwitch } from '../../ngxs-store/auth.actions';

@Component({
  selector: 'app-twitch-callback',
  standalone: true,
  imports: [CommonModule],
  template: '<p>Redirecting...</p>',
})
export class TwitchCallbackComponent implements OnInit {
  #route = inject(ActivatedRoute);
  #router = inject(Router);
  #ngxsStore = inject(Store);

  ngOnInit(): void {
    this.#route.queryParams.subscribe(params => {
      const code = params['code'];
      if (code) {
        // Call a service to send the authorization code to your backend
        this.#ngxsStore.dispatch(new CallbackTwitch(code)).subscribe({
          next: () => {
            this.#router.navigate([`/${ROUTES_PARAM.GROCERY_LIST.GROCERY_LIST}`]);
          },
          error: () => {
            this.#router.navigate(['/error']);
          }
        });
      } else {
        // Redirect to an error page or handle accordingly
        this.#router.navigate(['/error']);
      }
    });
  }
}
