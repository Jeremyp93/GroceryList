import { Routes } from "@angular/router";

import { authGuard } from "../../auth/auth.guard";
import { CategoryListComponent } from "./category-list/category-list.component";
import { CategoryComponent } from "./category.component";
import { ROUTES_PARAM } from "../../constants";
import { ProductListComponent } from "./product-list/product-list.component";

export const CATEGORY_ROUTES: Routes = [
    {
        path: '', component: CategoryComponent, canActivate: [authGuard], canActivateChild: [authGuard], title: 'Categories', children: [
            { path: '', component: CategoryListComponent, pathMatch: 'full' },
            { path: `:${ROUTES_PARAM.ID_PARAMETER}`, component: ProductListComponent, title: 'Categories - Productrs'},
        ]
    }
];