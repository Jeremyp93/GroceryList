import { Injectable, inject } from "@angular/core";
import { State, Selector, Action, StateContext } from "@ngxs/store";
import { tap } from "rxjs";


import { Product } from "../types/Product.type";
import { Category } from "../types/category.type";
import { ProductService } from "../product.service";
import { GetCategories, GetProducts, GetProductsByCategory } from "./product.action";

export interface ProductStateModel {
    products: Product[];
    selectedCategory: Category | null;
    categories: Category[];
    filteredProducts: Product[];
}

@State<ProductStateModel>({
    name: 'products',
    defaults: {
        products: [],
        selectedCategory: null,
        categories: [],
        filteredProducts: []
    }
})
@Injectable()
export class ProductState {
    productService = inject(ProductService);

    @Selector()
    static getProducts(state: ProductStateModel) {
        return state.products;
    }

    @Selector()
    static getSelectedCategory(state: ProductStateModel) {
        return state.selectedCategory;
    }

    @Selector()
    static getCategories(state: ProductStateModel): Category[] {
        return state.categories;
    }

    @Action(GetProducts)
    getProducts({ getState, setState }: StateContext<ProductStateModel>) {
        return this.productService.getAllProducts().pipe(
            tap((result) => {
                const state = getState();
                setState({
                    ...state,
                    products: result,
                });
            })
        );
    }

    @Action(GetProductsByCategory)
    getProductsByCategory({ getState, setState }: StateContext<ProductStateModel>, { categoryId }: GetProductsByCategory) {
        return this.productService.getAllProductsByCategory(categoryId).pipe(
            tap((result) => {
                const state = getState();
                setState({
                    ...state,
                    filteredProducts: result,
                });
            })
        );
    }

    @Action(GetCategories)
    getCategories({ getState, setState }: StateContext<ProductStateModel>) {
        return this.productService.getAllCategories().pipe(
            tap((result) => {
                const state = getState();
                setState({
                    ...state,
                    categories: result,
                });
            })
        );
    }
}