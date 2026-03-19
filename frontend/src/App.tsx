import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import Home from './pages/Home';
import LaunchesPage from './pages/LaunchesPage';
import Launch from './components/Launch';

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path='/' element={<Home />} />
        <Route path='/launches' element={<LaunchesPage />} />
        <Route path='/launch/:id' element={<Launch />} />
        <Route path='/launch/number/:flightNumber' element={<Launch />} />
        <Route path='*' element={<Navigate to='/' />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
