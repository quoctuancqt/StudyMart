import { create } from "zustand";

export type Product = {
    id: number;
    name: string;
    description: string;
    price: number;
    imageUrl: string;
    categoryName: string;
}

type ProductsState = {
    products: Product[];
    loading: boolean;
    error: string | null;
}

type ProductsStore = ProductsState & {
    fetchProducts: (products: Product[]) => void;
}

const initialState: ProductsState = {
    products: [],
    loading: false,
    error: null,
};

export const useProductsStore = create<ProductsStore>((set) => ({
    ...initialState,
    fetchProducts: (products) => set({ products }),
}));