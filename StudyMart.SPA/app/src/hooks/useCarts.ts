import apiClient from "@/api/apiClient";
import { CartItem, ShoppingCart, useCartStore } from "@/features/carts/cartsStore";
import { useMutation, UseMutationResult, useQuery, useQueryClient, UseQueryResult } from "@tanstack/react-query";

export const useFetchCarts = (): UseQueryResult<ShoppingCart[], Error> => useQuery({
    queryKey: ['carts'],
    queryFn: async (): Promise<ShoppingCart[]> => {
        const response = await apiClient.get('/carts');
        return response.data;
    },
    staleTime: 5 * 60 * 1000,
    refetchOnWindowFocus: true,
    refetchInterval: 10000,
});

export const useAddToCart = (): UseMutationResult<void, Error, CartItem> => {
    const queryClient = useQueryClient();
    const addToCart = useCartStore((state) => state.addToCart);
    return useMutation({
        mutationFn: async (item: CartItem) => {
            addToCart(item);
            return Promise.resolve();
        },
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['carts'] });
        }
    });
};