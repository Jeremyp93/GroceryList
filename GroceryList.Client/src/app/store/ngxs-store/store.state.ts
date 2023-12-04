import { inject } from "@angular/core";
import { State, Selector, Action, StateContext } from "@ngxs/store";
import { tap } from "rxjs";

import { StoreService } from "../store.service";
import { Store } from "../types/store.type";
import { GetStores } from "./store.actions";

export interface StoreStateModel {
    stores: Store[];
    selectedStore: Store | null;
}

@State<StoreStateModel>({
    name: 'groceryLists',
    defaults: {
        stores: [],
        selectedStore: null,
    }
})
export class StoreState {
    storeService = inject(StoreService);

    @Selector()
    static getStores(state: StoreStateModel) {
        return state.stores;
    }

    @Selector()
    static getSelectedStore(state: StoreStateModel) {
        return state.selectedStore;
    }

    @Action(GetStores)
    getStores({ getState, setState }: StateContext<StoreStateModel>) {
        return this.storeService.getAllStores().pipe(
            tap((result) => {
                const state = getState();
                setState({
                    ...state,
                    stores: result,
                });
            })
        );
    }
}