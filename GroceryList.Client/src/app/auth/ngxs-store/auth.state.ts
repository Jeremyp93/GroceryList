import { Injectable, inject } from "@angular/core";
import { Action, Selector, State, StateContext } from "@ngxs/store";
import { tap } from "rxjs";
import { JwtHelperService } from '@auth0/angular-jwt';

import { CallbackTwitch, GetMyself, Login, Logout, Register } from "./auth.actions";
import { AuthService } from "../auth.service";

export interface AuthStateModel {
    isAuthenticated: boolean;
    name: string | null;
    email: string | null;
}

@State<AuthStateModel>({
    name: 'auth',
    defaults: {
        isAuthenticated: false,
        name: null,
        email: null
    }
})

@Injectable()
export class AuthState {
    jwtHelper = inject(JwtHelperService);

    @Selector()
    static getName(state: AuthStateModel): string | null {
        return state.name;
    }

    @Selector()
    static isAuthenticated(state: AuthStateModel): boolean {
        return state.isAuthenticated;
    }

    constructor(private authService: AuthService) { }

    @Action(Login)
    login(ctx: StateContext<AuthStateModel>, action: Login) {
        return this.authService.login(action.payload).pipe(
            tap(() => {
                ctx.patchState({
                    isAuthenticated: true
                });
                ctx.dispatch(new GetMyself());
            })
        );
    }

    @Action(Register)
    register(_: StateContext<AuthStateModel>, action: Register) {
        return this.authService.register(action.payload);
    }

    @Action(Logout)
    logout(ctx: StateContext<AuthStateModel>) {
        return this.authService.logout().pipe(
            tap(() => {
                ctx.patchState({
                    isAuthenticated: false,
                    email: null,
                    name: null
                });
            })
        );
    }

    @Action(GetMyself)
    getMyself(ctx: StateContext<AuthStateModel>) {
        return this.authService.me().pipe(
            tap((result) => {
                ctx.patchState({
                    name: result?.lastName ? `${result.firstName} ${result?.lastName}` : `${result.firstName}`,
                    email: result.email,
                    isAuthenticated: true
                });
            })
        );
    }

    @Action(CallbackTwitch)
    callbackTwitch(ctx: StateContext<AuthStateModel>, {code}: CallbackTwitch) {
        return this.authService.loginTwitch(code).pipe(
            tap(() => {
                ctx.patchState({
                    isAuthenticated: true
                });
                ctx.dispatch(new GetMyself());
            })
        );
    }
}