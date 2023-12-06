import { inject } from "@angular/core";
import { State, Selector, Action, StateContext } from "@ngxs/store";
import { tap } from "rxjs";

import { StoreService } from "../store.service";
import { Store } from "../types/store.type";
import { AddStore, DeleteStore, GetSelectedStore, GetStores, SetSelectedStore, UpdateStore } from "./store.actions";
import { Section } from "../types/section.type";

export interface StoreStateModel {
    stores: Store[];
    selectedStore: Store | null;
    lastUpdatedStore: Store | null;
}

@State<StoreStateModel>({
    name: 'stores',
    defaults: {
        stores: [],
        selectedStore: null,
        lastUpdatedStore: null
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
    static getLastUpdatedStore(state: StoreStateModel) {
        return state.lastUpdatedStore;
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

    @Action(AddStore)
    addStore({ getState, patchState }: StateContext<StoreStateModel>, { payload }: AddStore) {
        return this.storeService.addStore(payload).pipe(
            tap((result) => {
                const state = getState();
                patchState({
                    stores: [...state.stores, result]
                });
            }));
    }

    @Action(GetSelectedStore)
    getSelectedStore({ getState, setState, dispatch }: StateContext<StoreStateModel>, { id }: GetSelectedStore) {
        return this.storeService.getStoreById(id).pipe(
            tap((result) => {
                const state = getState();
                setState({
                    ...state,
                    selectedStore: result
                });
            }));
    }

    @Action(UpdateStore)
    updateStore({ getState, setState }: StateContext<StoreStateModel>, { payload, id }: UpdateStore) {
        return this.storeService.updateStore(id, payload).pipe(
            tap((result) => {
                const state = getState();
                const stores = [...state.stores];
                const updatedStoreIndex = stores.findIndex(item => item.id === id);
                stores[updatedStoreIndex] = result;
                setState({
                    ...state,
                    stores: stores,
                    lastUpdatedStore: result
                });
            }));
    }
}