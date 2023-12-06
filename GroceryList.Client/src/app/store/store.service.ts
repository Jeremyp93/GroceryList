import { HttpClient } from "@angular/common/http";
import { Injectable, inject } from "@angular/core";
import { Observable, map } from "rxjs";

import { Store } from "./types/store.type";
import { environment } from "../../../src/environments/environment";
import { StoreResponseDto } from "./dtos/store.dto";

@Injectable({ providedIn: 'root' })
export class StoreService {
    httpClient = inject(HttpClient);

    getAllStores = (): Observable<Store[]> => {
        return this.httpClient.get<StoreResponseDto[]>(environment.storeApiUrl).pipe(map((dtos) => {
            return dtos.map(dto => (this.#fromStoreDto(dto)));
        }));
    }

    getStoreById = (id: string): Observable<Store> => {
        return this.httpClient.get<StoreResponseDto>(`${environment.storeApiUrl}/${id}`).pipe(map((dto) => this.#fromStoreDto(dto)));
    }

    deleteStore = (id: string): Observable<void> => {
        return this.httpClient.delete<void>(`${environment.storeApiUrl}/${id}`);
    }

    #fromStoreDto = (dto: StoreResponseDto): Store => {
        return {
            id: dto.id,
            name: dto.name,
            street: dto.street,
            city: dto.city,
            state: dto.state,
            country: dto.country,
            zipCode: dto.zipCode,
            sections: dto.sections,
            createdAt: dto.createdAt,
            showDelete: false
        }
    }
}