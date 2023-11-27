import { Login as LoginType } from "../types/login.type";

export class Login {
    static readonly type = '[Auth] Login';
    constructor(public payload: LoginType) { }
}

export class Logout {
    static readonly type = '[Auth] Logout';
}