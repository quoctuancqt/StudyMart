import Navbar from './components/ui/header';
import { Route, Routes } from 'react-router';
import Home from './pages/home';
import About from './pages/about';

function App() {
  return (
    <div className='container mx-auto'>
      <Navbar />
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/about" element={<About />} />
      </Routes>
    </div>
  )
}

export default App;
