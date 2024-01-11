import { Routes } from "@angular/router";

import { authGuard } from "../auth/auth.guard";
import { ItemsComponent } from "./items.component";
import { GroceryListComponent } from "../grocery-list/grocery-list.component";
import { ItemListComponent } from "./item-list/item-list.component";



export const ITEM_ROUTES: Routes = [
    {
        path: '', component: ItemsComponent, canActivate: [authGuard], canActivateChild: [authGuard], title: 'Items', children: [
            { path: '', component: ItemListComponent, pathMatch: 'full' },
            { path: 'test', component: GroceryListComponent },
        ]
    }
];