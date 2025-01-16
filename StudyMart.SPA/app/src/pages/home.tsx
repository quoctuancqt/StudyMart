import IconButton from '@/components/ui/icon-button';
import { useProductsStore } from '@/features/products/productsStore';
import { useFetchProducts } from '@/hooks/useProducts';
import { ShoppingCart } from 'lucide-react';
import { useEffect } from 'react'

const Home = () => {
    const { data } = useFetchProducts();
    const fetchProducts = useProductsStore((state) => state.fetchProducts);
    const { products } = useProductsStore();

    useEffect(() => {
        if (data) {
            fetchProducts(data);
        }
    }, [data]);

    const onAddToCart = () => {
        console.log('Added to cart');
    };

    return (
        <div className="grid gap-6 md:grid-cols-2 lg:grid-cols-3 lg:gap-8">
            {products.map((product) => (
                <a
                    key={product.id}
                    href="#"
                    className="flex flex-col overflow-clip rounded-xl border border-border"
                >
                    <div>
                        <img
                            src={product.imageUrl}
                            alt={product.name}
                            className="aspect-[16/9] h-full w-full object-contain object-center"
                        />
                    </div>
                    <div className="px-6 py-8 md:px-8 md:py-10 lg:px-10 lg:py-12">
                        <h3 className="mb-3 text-lg font-semibold md:mb-4 md:text-xl lg:mb-6">
                            {product.name}
                        </h3>
                        <p className="mb-3 text-muted-foreground md:mb-4 lg:mb-6">
                            {product.description}
                        </p>
                        <p className="flex items-center hover:underline">
                            <span className='flex-grow'></span>
                            <IconButton
                                onClick={onAddToCart}
                                icon={<ShoppingCart size={20} />}
                                className='ml-auto'
                            />
                        </p>
                    </div>
                </a>
            ))}
        </div>
    )
}

export default Home;