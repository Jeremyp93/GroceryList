import { Injectable, inject } from "@angular/core";
import { BehaviorSubject, Observable, of } from "rxjs";
import { HttpClient } from "@angular/common/http";
import { v4 as UUID } from 'uuid';

import { environment } from "../../environments/environment";
import { Item } from "./types/item";

@Injectable({providedIn: 'root'})
export class ItemsService {
    httpClient = inject(HttpClient);
    private items: BehaviorSubject<Item[]> = new BehaviorSubject<Item[]>([{id: '1', name: 'Banane'}, {id: '2', name: 'Pomme'}, {id: '3', name: 'Poire'}, {id: '4', name: 'Poulet'}, {id: '5', name: 'Lait'}, {id: '6', name: 'oeuf'}]);
    items$ = this.items.asObservable();

    getAllItems = () => {
        //return this.httpClient.delete<void>(`${environment.itemApiUrl}/${id}`);
    }

    addItem = (name: string) => {
        //this.httpClient.post<Item>(`${environment.itemApiUrl}`, {name: name}).subscribe(item => this.items.next([...this.items.value, item]));
        this.items.next([...this.items.value, {id: UUID(), name: name}])
    }

    deleteItem = (id: string) => {
        //return this.httpClient.delete<void>(`${environment.itemApiUrl}/${id}`);
        const filteredArray = this.items.value.filter(item => item.id !== id);
        console.log(filteredArray);
        this.items.next(filteredArray);
    }
}