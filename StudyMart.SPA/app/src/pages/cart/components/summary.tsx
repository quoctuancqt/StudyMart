import { Button } from '@/components/ui/button';
import { useCartStore } from '@/features/carts/cartsStore';
import Currency from '@/components/ui/currency';
import { useAuth } from 'react-oidc-context';
import { useNavigate } from 'react-router';
import apiClient from '@/api/apiClient';

const Summary = () => {
  const items = useCartStore((state) => state.cart.items);
  const auth = useAuth();
  const navigate = useNavigate();

  const totalPrice = items.reduce((total, item) => {
    return total + Number(item.price);
  }, 0);

  const onCheckout = async () => {
    if (!auth.isAuthenticated) {
      await auth.signinRedirect({ state: { returnUrl: window.location.pathname } });
    }
    else {
      await apiClient.post('/shopping-carts/batch', JSON.stringify(items)).then(() => {
        navigate('/checkout');
      });
    }
  };

  return (
    <div className='mt-16 rounded-lg border px-4 py-6 sm:p-6 lg:col-span-5 lg:mt-0 lg:p-8'>
      <h2 className='text-lg font-medium'>Order summary</h2>
      <div className='mt-6 space-y-4'>
        <div className='flex items-center justify-between border-t border-gray-200 pt-4'>
          <div className='text-base font-medium'>Order total</div>
          <Currency value={totalPrice} />
        </div>
      </div>
      <Button
        onClick={onCheckout}
        disabled={items.length === 0}
        className='w-full mt-6'
      >
        Checkout
      </Button>
    </div>
  );
};

export default Summary;