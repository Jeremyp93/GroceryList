import { Injectable, inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";

import { Login as LoginType } from "./types/login.type";
import { Register as RegisterType } from "./types/register.type";
import { environment } from "../../environments/environment";
import { ROUTES_PARAM } from "../constants";
import { UserResponseDto } from "./dtos/user-response-dto.type";

@Injectable({ providedIn: 'root' })
export class AuthService {
    httpClient = inject(HttpClient);

    login = (login: LoginType): Observable<void> => {
        return this.httpClient.post<void>(`${environment.userApiUrl}/${ROUTES_PARAM.AUTHENTICATION.LOGIN}`, { email: login.username, password: login.password });
    }

    register = (registerModel: RegisterType): Observable<void> => {
        return this.httpClient.post<void>(`${environment.userApiUrl}`, registerModel);
    }

    logout = (): Observable<void> => {
        return this.httpClient.post<void>(`${environment.userApiUrl}/${ROUTES_PARAM.AUTHENTICATION.LOGOUT}`, null);
    }

    me = (): Observable<UserResponseDto> => {
        return this.httpClient.get<UserResponseDto>(`${environment.userApiUrl}/${ROUTES_PARAM.AUTHENTICATION.ME}`);
    }

    loginTwitch = (code: string) => {
        return this.httpClient.post(`${environment.oauthApiUrl}/${ROUTES_PARAM.OAUTH.TWITCH_CALLBACK}`, {code: code});
    }
}