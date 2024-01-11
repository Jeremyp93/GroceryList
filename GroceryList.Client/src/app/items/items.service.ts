import { Injectable, inject } from "@angular/core";
import { BehaviorSubject, Observable, of } from "rxjs";
import { HttpClient } from "@angular/common/http";

import { environment } from "../../environments/environment";
import { Item } from "./types/item";

@Injectable({providedIn: 'root'})
export class ItemsService {
    httpClient = inject(HttpClient);
    private items: BehaviorSubject<Item[]> = new BehaviorSubject<Item[]>([]);
    items$ = this.items.asObservable();

    getAllItems = () => {
        this.httpClient.get<Item[]>(`${environment.itemApiUrl}`).subscribe(items => this.items.next(items));
    }

    addItem = (name: string) => {
        this.httpClient.post<Item>(`${environment.itemApiUrl}`, {name: name}).subscribe(item => this.items.next([...this.items.value, item]));
    }

    deleteItem = (id: string) => {
        this.httpClient.delete<void>(`${environment.itemApiUrl}/${id}`).subscribe(() => this.items.next([...this.items.value.filter(i => i.id !== id)]));
    }
}