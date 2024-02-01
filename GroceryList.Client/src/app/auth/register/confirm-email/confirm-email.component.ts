import { Component, OnInit, inject } from '@angular/core';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { filter, map } from 'rxjs';

@Component({
  selector: 'app-confirm-email',
  standalone: true,
  imports: [],
  templateUrl: './confirm-email.component.html',
  styleUrl: './confirm-email.component.scss'
})
export class ConfirmEmailComponent {
  #router = inject(Router);
  email = 'xxx';

  constructor(){
    const currentNav = this.#router.getCurrentNavigation();
    const state = currentNav?.extras.state as {email: string};
    this.email = state.email;
  }
}
