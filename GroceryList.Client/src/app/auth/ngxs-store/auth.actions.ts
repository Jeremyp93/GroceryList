import { Login as LoginType } from "../types/login.type";
import { Register as RegisterType } from "../types/register.type";

export class Login {
    static readonly type = '[Auth] Login';
    constructor(public payload: LoginType) { }
}

export class Register {
    static readonly type = '[Auth] Register';
    constructor(public payload: RegisterType) { }
}

export class Logout {
    static readonly type = '[Auth] Logout';
}

export class CallbackTwitch {
    static readonly type = '[Auth] Callback Twitch';
    constructor(public code: string) { }
}

export class CallbackGoogle {
    static readonly type = '[Auth] Callback Google';
    constructor(public code: string) { }
}

export class GetMyself {
    static readonly type = '[Auth] Me';
}