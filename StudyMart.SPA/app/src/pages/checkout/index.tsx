import Currency from "@/components/ui/currency";
import { CartItem as CartItemType, useCartStore } from "@/features/carts/cartsStore";
import CartItem from "../cart/components/cart-item";
import BillingInfoForm from "./components/billing-info";
import apiClient from "@/api/apiClient";
import { useNavigate } from 'react-router';

const CheckoutPage = () => {
    const items = useCartStore((state) => state.cart.items);
    const totalPrice = items.reduce((total, item) => {
        return total + Number(item.price);
    }, 0);
    const navigate = useNavigate();
    const clearCart = useCartStore((state) => state.clearCart);

    const onSubmit = async (formValues: any) => {
        await apiClient.post('/orders', JSON.stringify(formValues)).then(() => {
            clearCart();
            navigate('/order-confirmation');
        });
    };

    return (
        <div className='px-4 sm:px-6 lg:px-8'>
            <div className='mt-12 lg:grid lg:grid-cols-12 lg:items-start gap-x-12'>
                <BillingInfoForm onSubmitCallback={onSubmit}/>
                <div className='mt-16 rounded-lg border px-4 py-6 sm:p-6 lg:col-span-5 lg:mt-0 lg:p-8'>
                    <h2 className='text-lg font-medium'>Order summary</h2>
                    <ul>
                        {items.map((item: CartItemType) => (
                            <CartItem key={item.productId} data={item} />
                        ))}
                    </ul>
                    <div className='mt-6 space-y-4'>
                        <div className='flex items-center justify-between border-t border-gray-200 pt-4'>
                            <div className='text-base font-medium'>Order total</div>
                            <Currency value={totalPrice} />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
}

export default CheckoutPage;