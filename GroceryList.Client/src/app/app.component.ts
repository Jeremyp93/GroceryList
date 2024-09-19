import { Component, HostListener, OnDestroy, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { Actions, Store, ofActionCompleted } from '@ngxs/store';
import { Subscription } from 'rxjs';

import { ROUTES_PARAM } from './constants';
import { AuthState } from './auth/ngxs-store/auth.state';
import { Logout } from './auth/ngxs-store/auth.actions';
import { AlertComponent } from './shared/alert/alert.component';
import { ConfirmDialogComponent } from './shared/confirm-dialog/confirm-dialog.component';
import { SideMenuComponent } from './menu/side-menu/side-menu.component';
import { SideMenuService } from './menu/side-menu/side-menu.service';
import { GetCategories } from './new/category/ngxs-store/product.action';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, RouterModule, AlertComponent, ConfirmDialogComponent, SideMenuComponent],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit, OnDestroy {
  #router = inject(Router);
  #actions = inject(Actions);
  #ngxsStore = inject(Store);
  #sideMenuService = inject(SideMenuService);
  isKeyboardOpen = false;
  previousHeight: number = 0;
  groceryListRoute: string = '/' + ROUTES_PARAM.GROCERY_LIST.GROCERY_LIST;
  storeRoute: string = '/' + ROUTES_PARAM.STORE.STORE;
  showMenu: boolean = false;
  showSidebar: boolean = false;
  #sideMenuSubscription!: Subscription;

  @HostListener('window:resize')
  onResize() {
    const currentHeight = window.innerHeight;
    const heightDifference = Math.abs(currentHeight - this.previousHeight);

    const threshold = 300;

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
    this.#actions.pipe(ofActionCompleted(Logout)).subscribe(() => {
      this.#router.navigate([`/${ROUTES_PARAM.AUTHENTICATION.AUTH}/${ROUTES_PARAM.AUTHENTICATION.LOGIN}`]);
    });

    this.#ngxsStore.select(AuthState.isAuthenticated).subscribe(value => this.showMenu = value);

    this.#sideMenuSubscription = this.#sideMenuService.isOpen$.subscribe(open => this.showSidebar = open);
    this.#ngxsStore.dispatch(new GetCategories());
  }

  onLogout = () => {
    this.#ngxsStore.dispatch(new Logout());
  }

  openCloseSidebar = () => {
    if (this.showSidebar) {
      this.#sideMenuService.closeMenu();
    } else {
      this.#sideMenuService.openMenu();
    }
  }
  ngOnDestroy() {
    if (this.#sideMenuSubscription) {
      this.#sideMenuSubscription.unsubscribe();
    }
  }
}
