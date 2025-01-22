import { useQuery, UseQueryResult } from '@tanstack/react-query';
import apiClient from '../api//apiClient';
import { Product } from '../features/products/productsStore';

export const useFetchProducts = (): UseQueryResult<Product[], Error> => useQuery({
  queryKey: ['products'],
  // The queryFn in React Query is called under several conditions:
  //  1. Initial Load: When the component that uses the useQuery hook mounts for the first time.
  //  2. Stale Data: When the data in the cache is considered stale. This is determined by the staleTime configuration.
  //  3. Window Focus: When the window regains focus, if refetchOnWindowFocus is set to true.
  //  4. Interval Refetching: At regular intervals, if refetchInterval is set.
  //  5. Manual Refetch: When you manually refetch the query using methods like queryClient.invalidateQueries or queryClient.refetchQueries.
  //  6. Network Reconnect: When the network reconnects, if refetchOnReconnect is set to true.
  queryFn: async (): Promise<Product[]> => {
    const response = await apiClient.get('/products');
    return response.data;
  },
  staleTime: 5 * 60 * 1000, // Cache for 5 minutes (default: 0)
  refetchOnWindowFocus: true, // Refetch on focus (default: true)
  refetchInterval: 10000, // Polling every 10 seconds (default: false)
});