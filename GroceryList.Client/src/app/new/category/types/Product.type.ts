import { Category } from "./category.type";

export type Product = {
    id: string;
    name: string;
    productId: string;
    brand: string;
    category: Category;
    price: number;
    thumbnail: string;
    picture: string;
}