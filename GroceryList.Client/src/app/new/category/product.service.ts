import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from "../../../environments/environment";
import { Category } from "./types/category.type";

@Injectable({ providedIn: 'root' })
export class ProductService {
    httpClient = inject(HttpClient);

    getAllCategories = (): Observable<Category[]> => {
        return this.httpClient.get<Category[]>(`${environment.apiUrl}/${environment.categoriesEndpoint}`);
    }
}