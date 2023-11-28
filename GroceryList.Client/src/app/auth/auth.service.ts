import { Injectable, inject } from "@angular/core";
import { HttpClient, HttpErrorResponse } from "@angular/common/http";
import { Observable, catchError, throwError } from "rxjs";

import { Login as LoginType } from "./types/login.type";
import { Register as RegisterType } from "./types/register.type";
import { environment } from "../../environments/environment";
import { ROUTES_PARAM } from "../constants";
import { TokenResponseDto } from "./dtos/token-response-dto.type";

@Injectable({ providedIn: 'root' })
export class AuthService {
    httpClient = inject(HttpClient);

    login = (login: LoginType): Observable<TokenResponseDto> => {
        return this.httpClient.post<TokenResponseDto>(`${environment.userApiUrl}/${ROUTES_PARAM.LOGIN}`, { email: login.username, password: login.password }).pipe(catchError(this.#handleError));
    }

    register = (registerModel: RegisterType): Observable<void> => {
        return this.httpClient.post<void>(`${environment.userApiUrl}`, registerModel).pipe(catchError(this.#handleError));
    }

    #handleError = (errorResponse: HttpErrorResponse) => {
        let errorMessage = 'An unknown error occured.';
        if (!Array.isArray(errorResponse.error)) {
            return throwError(() => new Error(errorMessage));
        }

        errorMessage = errorResponse.error[0];
        return throwError(() => new Error(errorMessage));
    }
}