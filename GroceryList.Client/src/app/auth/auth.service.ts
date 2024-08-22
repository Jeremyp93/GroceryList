import { Injectable, inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";

import { Login as LoginType } from "./types/login.type";
import { Register as RegisterType } from "./types/register.type";
import { environment } from "../../environments/environment";
import { API_ENDPOINTS } from "../constants";
import { UserResponseDto } from "./dtos/user-response-dto.type";
import { ResetPasswordRequestDto } from "./dtos/reset-password.dto";

@Injectable({ providedIn: 'root' })
export class AuthService {
    httpClient = inject(HttpClient);

    login = (login: LoginType): Observable<void> => {
        return this.httpClient.post<void>(`${environment.apiUrl}/${environment.userEndpoint}/${API_ENDPOINTS.LOGIN}`, { email: login.username, password: login.password });
    }

    register = (registerModel: RegisterType): Observable<void> => {
        return this.httpClient.post<void>(`${environment.apiUrl}/${environment.userEndpoint}`, registerModel);
    }

    logout = (): Observable<void> => {
        return this.httpClient.post<void>(`${environment.apiUrl}/${environment.userEndpoint}/${API_ENDPOINTS.LOGOUT}`, null);
    }

    me = (): Observable<UserResponseDto> => {
        return this.httpClient.get<UserResponseDto>(`${environment.apiUrl}/${environment.userEndpoint}/${API_ENDPOINTS.ME}`);
    }

    loginTwitch = (code: string) => {
        return this.httpClient.post(`${environment.apiUrl}/${environment.oauthEndpoint}/${API_ENDPOINTS.TWITCH_CALLBACK}`, {code: code});
    }

    loginGoogle = (code: string) => {
        return this.httpClient.post(`${environment.apiUrl}/${environment.oauthEndpoint}/${API_ENDPOINTS.GOOGLE_CALLBACK}`, {code: code});
    }

    confirmEmail = (token: string, email: string) => {
        return this.httpClient.post(`${environment.apiUrl}/${environment.userEndpoint}/${API_ENDPOINTS.CONFIRM}`, {token, email});
    }

    resetPassword = (resetPasswordRequest: ResetPasswordRequestDto) => {
        return this.httpClient.post<void>(`${environment.apiUrl}/${environment.userEndpoint}/${API_ENDPOINTS.RESET_PASSWORD}`, resetPasswordRequest);
    }

    forgotPassword = (email: string) => {
        return this.httpClient.post<void>(`${environment.apiUrl}/${environment.userEndpoint}/${email}/${API_ENDPOINTS.FORGOT_PASSWORD}`, {});
    }
}