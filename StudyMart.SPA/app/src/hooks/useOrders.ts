import apiClient from "@/api/apiClient";
import { Order } from "@/types/order/order";
import { useQuery, UseQueryResult } from "@tanstack/react-query";

export const useFetchOrders = (): UseQueryResult<Order[], Error> => useQuery({
    queryKey: ['orders'],
    queryFn: async (): Promise<Order[]> => {
        const response = await apiClient.get('/orders');
        return response.data;
    },
    staleTime: 5 * 60 * 1000,
    refetchOnWindowFocus: true,
    refetchInterval: 10000,
});