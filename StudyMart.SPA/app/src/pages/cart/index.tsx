import { useEffect, useState } from 'react';

import { CartItem as CartItemType, useCartStore } from '@/features/carts/cartsStore';
import CartItem from './components/cart-item';
import Summary from './components/summary';

export const revalidate = 0;

const CartPage = () => {
  const [isMounted, setIsMounted] = useState(false);
  const { cart } = useCartStore();

  useEffect(() => {
    setIsMounted(true);
  }, []);

  if (!isMounted) {
    return null;
  }

  return (
    <div className='px-4 py-16 sm:px-6 lg:px-8'>
      <h1 className='text-3xl font-bold'>Shopping Cart</h1>
      <div className='mt-12 lg:grid lg:grid-cols-12 lg:items-start gap-x-12'>
        <div className='lg:col-span-7'>
          {cart.items.length === 0 && (
            <p className='text-neutral-500'>No items added to cart.</p>
          )}
          <ul>
            {cart.items.map((item: CartItemType) => (
              <CartItem key={item.productId} data={item} hasRemove={true} />
            ))}
          </ul>
        </div>
        <Summary />
      </div>
    </div>
  );
};

export default CartPage;