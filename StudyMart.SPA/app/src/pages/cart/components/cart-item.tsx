import { X } from 'lucide-react';

import IconButton from '@/components/ui/icon-button';
import { useCartStore, type CartItem } from '@/features/carts/cartsStore';
import Currency from '@/components/ui/currency';

interface CartItemProps {
    data: CartItem;
}

const CartItem: React.FC<CartItemProps> = ({ data }) => {
    const removeFromCart = useCartStore((state) => state.removeFromCart);
    const onRemove = (productId: number) => {
        removeFromCart(productId);
    };

    return (
        <li className='flex py-6 border-b'>
            {/* <div className='relative h-24 w-24 rounded-md overflow-hidden sm:h-48 sm:w-48'>
        <img src={data.imag} alt='' className='object-cover object-center' />
      </div> */}
            <div className='relative ml-4 flex flex-1 flex-col justify-between sm:ml-6'>
                <div className='absolute z-10 right-0 top-0'>
                    <IconButton onClick={() => onRemove(data.productId)} icon={<X size={15} />} />
                </div>
                <div className='relative pr-9 sm:grid sm:grid-cols-2 sm:gap-x-6 sm:pr-0'>
                    <div className='flex justify-between'>
                        <p className=' text-lg font-semibold'>{data.name}</p>
                    </div>

                    <div className='mt-1 flex text-sm'>
                        {/* <p className='text-gray-500'>{data.color.name}</p> */}
                        <p className='ml-4 border-l border-gray-200 pl-4 text-gray-500'>
                            {/* {data.size.name} */}
                        </p>
                    </div>
                    <Currency value={data.price} />
                </div>
            </div>
        </li>
    );
};

export default CartItem;