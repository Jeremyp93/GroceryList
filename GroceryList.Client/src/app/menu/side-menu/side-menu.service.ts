import { Injectable, Renderer2, inject } from "@angular/core";
import { BehaviorSubject } from "rxjs";

@Injectable({providedIn: 'root'})
export class SideMenuService {
    #isOpen = new BehaviorSubject<boolean>(false);
    isOpen$ = this.#isOpen.asObservable();
    
    openMenu = () => {
        this.#isOpen.next(true);
    }

    closeMenu = () => {
        this.#isOpen.next(false);
    }
}