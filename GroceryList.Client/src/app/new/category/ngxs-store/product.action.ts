export class GetProducts {
    static readonly type = '[Product] Get';
}

export class GetProductsByCategory {
    static readonly type = '[Product] Get By Category';

    constructor(public categoryId: string) {
    }
}

export class GetCategories {
    static readonly type = '[Product] Get Categories';
}