import { Store } from "../types/store.type";

export class GetStores {
    static readonly type = '[Store] Get';
}

export class DeleteStore {
    static readonly type = '[Store] Delete';

    constructor(public id: string) {
    }
}

export class SetSelectedStore {
    static readonly type = '[Store] Set';

    constructor(public payload: Store | null) {
    }
}