import { Injectable } from "@angular/core";
import { Action, Selector, State, StateContext } from "@ngxs/store";
import { Login, Logout } from "./auth.actions";
import { AuthService } from "../auth.service";
import { catchError, tap, throwError } from "rxjs";
import { TokenResponseDto } from "../dtos/token-response-dto.type";

export interface AuthStateModel {
    token: string | null;
    username: string | null;
}

@State<AuthStateModel>({
    name: 'auth',
    defaults: {
        token: null,
        username: null
    }
})

@Injectable()
export class AuthState {
    @Selector()
    static token(state: AuthStateModel): string | null {
        return state.token;
    }

    @Selector()
    static username(state: AuthStateModel): string | null {
        return state.username;
    }

    @Selector()
    static isAuthenticated(state: AuthStateModel): boolean {
        return !!state.token;
    }

    constructor(private authService: AuthService) { }

    @Action(Login)
    login(ctx: StateContext<AuthStateModel>, action: Login) {
        return this.authService.login(action.payload).pipe(
            catchError(error => {
                console.log(error);
                return throwError(() => Error('test'));
            }),
            tap((result: TokenResponseDto) => {
                console.log(result);
                ctx.patchState({
                    token: result.token,
                    username: action.payload.username
                });
            })
        );
    }

    @Action(Logout)
    logout(ctx: StateContext<AuthStateModel>) {
        ctx.setState({
            token: null,
            username: null
        });
    }
}