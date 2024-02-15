import { Routes } from '@angular/router';

import { ROUTES_PARAM } from './constants';
import { TwitchCallbackComponent } from './auth/callback/twitch-callback/twitch-callback.component';
import { ErrorComponent } from './error.component';

export const routes: Routes = [
    { path: '', redirectTo: ROUTES_PARAM.GROCERY_LIST.GROCERY_LIST, pathMatch: 'full' },
    {
        path: ROUTES_PARAM.GROCERY_LIST.GROCERY_LIST,
        loadChildren: () => import('./grocery-list/grocery-list.routes').then(m => m.GROCERY_LIST_ROUTES),
    },
    {
        path: ROUTES_PARAM.STORE.STORE,
        loadChildren: () => import('./store/store.routes').then(m => m.STORE_ROUTES),
    },
    {
        path: ROUTES_PARAM.ITEMS.ITEMS,
        loadChildren: () => import('./items/items.routes').then(m => m.ITEM_ROUTES),
    },
    {
        path: ROUTES_PARAM.AUTHENTICATION.AUTH,
        loadChildren: () => import('./auth/auth.routes').then(m => m.AUTH_ROUTES),
    },
    {
        path: 'error', component: ErrorComponent, title: 'Error'
    },
    { path: '**', redirectTo: ROUTES_PARAM.GROCERY_LIST.GROCERY_LIST }
];
