import { Routes } from "@angular/router";

import { authGuard } from "../../auth/auth.guard";
import { CategoryListComponent } from "./category-list/category-list.component";
import { CategoryComponent } from "./category.component";

export const CATEGORY_ROUTES: Routes = [
    {
        path: '', component: CategoryComponent, canActivate: [authGuard], canActivateChild: [authGuard], title: 'Categories', children: [
            { path: '', component: CategoryListComponent, pathMatch: 'full' }
        ]
    }
];