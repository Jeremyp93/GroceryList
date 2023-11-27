import { Component, HostListener, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { ROUTES_PARAM } from './constants';
import { Actions, Select, Store, ofActionDispatched } from '@ngxs/store';
import { Login, Logout } from './auth/ngxs-store/auth.actions';
import { AuthState } from './auth/ngxs-store/auth.state';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  router = inject(Router);
  actions = inject(Actions);
  ngxsStore = inject(Store);
  @Select(AuthState.username) username$!: Observable<string>;
  isKeyboardOpen = false;
  previousHeight: number = 0;
  groceryListRoute: string = '/' + ROUTES_PARAM.GROCERY_LIST;
  storeRoute: string = '/' + ROUTES_PARAM.STORE;
  showMenu: boolean = false;

  @HostListener('window:resize')
  onResize() {
    const currentHeight = window.innerHeight;
    const heightDifference = Math.abs(currentHeight - this.previousHeight);

    const threshold = 100;

    if (heightDifference > threshold) {
      if (currentHeight < this.previousHeight) {
        this.isKeyboardOpen = true;
      } else {
        this.isKeyboardOpen = false;
      }
      this.previousHeight = currentHeight;
    }
  }

  ngOnInit() {
    this.actions.pipe(ofActionDispatched(Logout)).subscribe(() => {
      this.router.navigate([`/${ROUTES_PARAM.LOGIN}`]);
    });

    this.ngxsStore.select(AuthState.isAuthenticated).subscribe(value => this.showMenu = value);
  }

  onLogout = () => {
    this.ngxsStore.dispatch(new Logout());
  }
}
