import { Routes } from '@angular/router';

import { GroceryListDetailsComponent } from './grocery-list/grocery-list-details/grocery-list-details.component';
import { GroceryListItemsComponent } from './grocery-list/grocery-list-items/grocery-list-items.component';
import { GroceryListComponent } from './grocery-list/grocery-list.component';
import { GroceryListNewComponent } from './grocery-list/grocery-list-items/grocery-list-new/grocery-list-new.component';
import { validIdGuard } from './guards/validId-guard.service';
import { StoreComponent } from './store/store.component';
import { StoreListComponent } from './store/store-list/store-list.component';
import { ROUTES_PARAM } from './constants';
import { authGuard } from './auth/auth.guard';
import { AuthComponent } from './auth/auth.component';
import { RegisterComponent } from './auth/register/register.component';

export const routes: Routes = [
    { path: '', redirectTo: ROUTES_PARAM.GROCERY_LIST, pathMatch: 'full' },
    {
        path: ROUTES_PARAM.GROCERY_LIST, component: GroceryListComponent, canActivate: [authGuard], canActivateChild: [authGuard], children: [
            { path: '', component: GroceryListItemsComponent, pathMatch: 'full' },
            { path: ROUTES_PARAM.NEW, component: GroceryListNewComponent },
            { path: `:${ROUTES_PARAM.ID_PARAMETER}`, component: GroceryListDetailsComponent, canActivate: [validIdGuard] },
            { path: `:${ROUTES_PARAM.ID_PARAMETER}/${ROUTES_PARAM.EDIT}`, component: GroceryListNewComponent, canActivate: [validIdGuard] },
        ]
    },
    {
        path: ROUTES_PARAM.STORE, component: StoreComponent, canActivate: [authGuard], canActivateChild: [authGuard], children: [
            { path: '', component: StoreListComponent, pathMatch: 'full' },
        ]
    },
    {
        path: ROUTES_PARAM.LOGIN, component: AuthComponent
    },
    {
        path: ROUTES_PARAM.REGISTER, component: RegisterComponent
    }
];
