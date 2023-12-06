import { inject } from "@angular/core";
import { State, Selector, Action, StateContext } from "@ngxs/store";
import { tap } from "rxjs";

import { StoreService } from "../store.service";
import { Store } from "../types/store.type";
import { DeleteStore, GetStores, SetSelectedStore } from "./store.actions";
import { Section } from "../types/section.type";

export interface StoreStateModel {
    stores: Store[];
    selectedStore: Store | null;
}

@State<StoreStateModel>({
    name: 'stores',
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

    @Selector()
    static getSections(state: StoreStateModel): Section[] {
        return state.selectedStore?.sections ?? [];
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

    @Action(DeleteStore)
    deleteStore({ getState, setState }: StateContext<StoreStateModel>, { id }: DeleteStore) {
        return this.storeService.deleteStore(id).pipe(
            tap(() => {
                const state = getState();
                const filteredArray = state.stores.filter(item => item.id !== id);
                setState({
                    ...state,
                    stores: filteredArray,
                });
            }));
    }

    @Action(SetSelectedStore)
    setSelectedStore({ getState, setState, dispatch }: StateContext<StoreStateModel>, { payload }: SetSelectedStore) {
        const state = getState();
        setState({
            ...state,
            selectedStore: payload
        });
    }
}