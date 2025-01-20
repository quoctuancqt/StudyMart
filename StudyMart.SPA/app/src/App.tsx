
import { Route, Routes } from 'react-router';
import Home from './pages/home';
import Navbar from './components/Header';
import CartPage from './pages/cart';
import CheckoutPage from './pages/checkout';
import OrderPage from './pages/order';
import OrderConfirmationPage from './pages/cart/components/confirmation';

function App() {
  return (
    <div className='w-screen container mx-auto'>
      <Navbar />
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path='/cart' element={<CartPage />} />
        <Route path='/checkout' element={<CheckoutPage />} />
        <Route path='/orders' element={<OrderPage />} />
        <Route path='/order-confirmation' element={<OrderConfirmationPage />} />
      </Routes>
    </div>
  )
}

export default App;
