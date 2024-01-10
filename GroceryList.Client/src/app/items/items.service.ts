import { Injectable, inject } from "@angular/core";
import { Observable, of } from "rxjs";
import { HttpClient } from "@angular/common/http";

import { environment } from "../../environments/environment";
import { Item } from "./types/item";

@Injectable({providedIn: 'root'})
export class ItemsService {
    httpClient = inject(HttpClient);
    
    getAllItems = (): Observable<Item[]> => {
        //return this.httpClient.get<Item[]>(environment.itemApiUrl);
        return of([{id: '1', name: 'Banane'}, {id: '2', name: 'Pomme'}, {id: '3', name: 'Poire'}, {id: '4', name: 'Poulet'}, {id: '5', name: 'Lait'}, {id: '6', name: 'oeuf'}]);
    }
}