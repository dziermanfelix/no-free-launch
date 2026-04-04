import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import Home from './pages/Home';
import LaunchesPage from './pages/LaunchesPage';
import Launch from './components/Launch';
import Login from './pages/Login';
import Register from './pages/Register';
import ProtectedRoute from './components/ProtectedRoute';

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path='/login' element={<Login />} />
        <Route path='/register' element={<Register />} />
        <Route element={<ProtectedRoute />}>
          <Route path='/' element={<Home />} />
          <Route path='/launches' element={<LaunchesPage />} />
          <Route path='/launch/:id' element={<Launch />} />
          <Route path='/launch/number/:flightNumber' element={<Launch />} />
        </Route>
        <Route path='*' element={<Navigate to='/' />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
