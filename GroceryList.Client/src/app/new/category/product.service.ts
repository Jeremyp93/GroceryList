import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from "../../../environments/environment";
import { Category } from "./types/category.type";
import { Product } from "./types/Product.type";

@Injectable({ providedIn: 'root' })
export class ProductService {
    httpClient = inject(HttpClient);

    getAllCategories = (): Observable<Category[]> => {
        return this.httpClient.get<Category[]>(`${environment.apiUrl}/${environment.categoryEndpoint}`);
    }

    getAllProducts = (): Observable<Product[]> => {
        return this.httpClient.get<Product[]>(`${environment.apiUrl}/${environment.productEndpoint}`);
    }

    getAllProductsByCategory = (categoryId: string): Observable<Product[]> => {
        return this.httpClient.get<Product[]>(`${environment.apiUrl}/${environment.categoryEndpoint}/${categoryId}/${environment.productEndpoint}`);
    }
}