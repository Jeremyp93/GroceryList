import { Injectable, inject, signal } from "@angular/core";
import { HttpClient } from "@angular/common/http";

import { environment } from "../../environments/environment";
import { Item } from "./types/item";

@Injectable({providedIn: 'root'})
export class ItemsService {
    httpClient = inject(HttpClient);
    #itemsS = signal<Item[]>([]);
    items = this.#itemsS.asReadonly();

    getAllItems = () => {
        this.httpClient.get<Item[]>(`${environment.apiUrl}/${environment.itemEndpoint}`).subscribe(items => this.#itemsS.set(items));
    }

    addItem = (name: string) => {
        this.httpClient.post<Item>(`${environment.apiUrl}/${environment.itemEndpoint}`, {name: name}).subscribe(item => this.#itemsS.set([...this.#itemsS(), item]));
    }

    deleteItem = (id: string) => {
        this.httpClient.delete<void>(`${environment.apiUrl}/${environment.itemEndpoint}/${id}`).subscribe(() => this.#itemsS.set([...this.#itemsS().filter(i => i.id !== id)]));
    }
}