import { Routes } from "@angular/router";

import { ROUTES_PARAM } from "../constants";
import { AuthComponent } from "./auth.component";
import { LoginComponent } from "./login/login.component";
import { RegisterComponent } from "./register/register.component";
import { ForgotPasswordComponent } from "./login/forgot-password/forgot-password.component";
import { ResetPasswordComponent } from "./reset-password/reset-password.component";
import { ConfirmEmailCallbackComponent } from "./callback/confirm-email-callback.component";
import { ConfirmEmailComponent } from "./register/confirm-email/confirm-email.component";
import { TwitchCallbackComponent } from "./callback/twitch-callback/twitch-callback.component";

export const AUTH_ROUTES: Routes = [
    {
        path: '', component: AuthComponent, children: [
            { path: '', redirectTo: `/${ROUTES_PARAM.GROCERY_LIST.GROCERY_LIST}`, pathMatch: 'full' },
            { path: ROUTES_PARAM.AUTHENTICATION.LOGIN, component: LoginComponent, title: 'Auth - Login' },
            { path: ROUTES_PARAM.AUTHENTICATION.REGISTER, component: RegisterComponent, title: 'Auth - Sign Up' },
            { path: ROUTES_PARAM.AUTHENTICATION.FORGOT_PASSWORD, component: ForgotPasswordComponent, title: 'Auth - Forgot Password' },
            { path: ROUTES_PARAM.AUTHENTICATION.RESET_PASSWORD, component: ResetPasswordComponent, title: 'Auth - Reset Password' },
            { path: ROUTES_PARAM.AUTHENTICATION.CONFIRM_EMAIL, component: ConfirmEmailCallbackComponent, title: 'Redirecting...' },
            { path: ROUTES_PARAM.AUTHENTICATION.EMAIL_CONFIRM_INFO, component: ConfirmEmailComponent, title: 'Auth - Cofirm Email Info' },
            { path: 'twitch-callback', component: TwitchCallbackComponent, title: 'Redirecting...' },
        ]
    },
];