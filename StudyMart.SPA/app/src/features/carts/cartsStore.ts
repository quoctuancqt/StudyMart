import { create } from "zustand";
import { createJSONStorage, persist } from 'zustand/middleware'

export type CartItem = {
    quantity: number;
    name: string;
    price: number;
    productId: number;
}

export type ShoppingCart = {
    items: CartItem[];
    id: number;
}

type CartState = {
    cart: ShoppingCart;
    totalItems: number;
}

type CartStore = CartState & {
    addToCart: (item: CartItem) => void;
    fetchCart: (cart: ShoppingCart) => void;
}

const initialState: CartState = {
    cart: {
        items: [],
        id: 0,
    },
    totalItems: 0,
}

export const useCartStore = create<CartStore>()(
    persist(
        (set) => ({
            ...initialState,
            addToCart: (item) => set((state) => {
                const existingItemIndex = state.cart.items.findIndex(cartItem => cartItem.productId === item.productId);
                let updatedItems;
                if (existingItemIndex !== -1) {
                    // Item exists, increase quantity
                    updatedItems = state.cart.items.map((cartItem, index) => 
                        index === existingItemIndex 
                            ? { ...cartItem, quantity: cartItem.quantity + 1 } 
                            : cartItem
                    );
                } else {
                    // Item does not exist, add new item
                    updatedItems = [...state.cart.items, { ...item, quantity: 1 }];
                }
                return { cart: { ...state.cart, items: updatedItems } };
            }),
            fetchCart: (cart) => set({ cart })
        }),
        {
            name: 'cart-storage', 
            storage: createJSONStorage(() => localStorage), 
        }
    )
);