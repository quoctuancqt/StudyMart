import { create } from "zustand";

export type Item = {
    id: number;
    title: string;
}

type ItemsState = {
    items: Item[];
    loading: boolean;
    error: string | null;
}

type ItemsStore = ItemsState & {
    fetchItems: (items: Item[]) => void;
    addItem: (item: Item) => void;
    updateItem: (item: Item) => void;
    removeItem: (id: number) => void;
}

const initialState: ItemsState = {
    items: [],
    loading: false,
    error: null,
};

export const useItemsStore = create<ItemsStore>((set) => ({
    ...initialState,
    fetchItems: (items) => set({ items }),
    addItem: (item) => set((state) => ({ items: [...state.items, item] })),
    updateItem: (item) => set((state) => ({
        items: state.items.map((i) => (i.id === item.id ? item : i)),
    })),
    removeItem: (id) => set((state) => ({
        items: state.items.filter((i) => i.id !== id),
    })),
}));