import { inject } from "@angular/core";
import { CanActivateFn, Router } from "@angular/router";

import { Store } from "@ngxs/store";
import { AuthState } from "./ngxs-store/auth.state";
import { Logout } from "./ngxs-store/auth.actions";

export const authGuard: CanActivateFn = () => {
    const store = inject(Store);

    const isAuthenticated = store.selectSnapshot(AuthState.isAuthenticated);

    if (!isAuthenticated) {
        store.dispatch(new Logout());
        return false;
    }

    return true;
};