import { Button } from '@/components/ui/button';
// import { toast } from '@/components/ui/use-toast';
import { useCartStore } from '@/features/carts/cartsStore';
import Currency from '@/components/ui/currency';

const Summary = () => {
  const items = useCartStore((state) => state.cart.items);

  const totalPrice = items.reduce((total, item) => {
    return total + Number(item.price);
  }, 0);

  const onCheckout = async () => {
    // const response = await axios.post(
    //   `${process.env.NEXT_PUBLIC_API_URL}/checkout`,
    //   {
    //     productIds: items.map((item) => item.id),
    //   }
    // );

    // window.location = response.data.url;
    console.log('Checkout');
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