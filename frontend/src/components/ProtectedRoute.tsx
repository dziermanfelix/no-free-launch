import { Navigate, Outlet } from 'react-router-dom';
import { useAuth } from '../contexts/AuthContext';

export default function ProtectedRoute() {
  const { isAuthenticated } = useAuth();

  console.log('isauth', isAuthenticated)

  if (!isAuthenticated) {
    return <Navigate to='/login' />;
  }

  return <Outlet />;
}
