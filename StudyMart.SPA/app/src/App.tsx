
import { Route, Routes } from 'react-router';
import Home from './pages/home';
import Navbar from './components/Header';
import CartPage from './pages/cart';
import CheckoutPage from './pages/checkout';
import OrderPage from './pages/order';

function App() {
  return (
    <div className='container mx-auto'>
      <Navbar />
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path='/cart' element={<CartPage />} />
        <Route path='/checkout' element={<CheckoutPage />} />
        <Route path='/orders' element={<OrderPage />} />
      </Routes>
    </div>
  )
}

export default App;
