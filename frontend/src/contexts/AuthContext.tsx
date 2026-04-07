import { createContext, useContext, useEffect, useMemo, useState } from 'react';
import type { ReactNode } from 'react';
import type { User } from '../types/user';
import { useQuery } from '@apollo/client/react';
import { getStoredToken, removeStoredToken, setStoredToken } from '../util/token';
import { ME } from '../graphql/queries';

type AuthContextValue = {
  user: User | null;
  token: string | null;
  isAuthenticated: boolean;
  isLoading: boolean;
  login: (user: User, token: string) => void;
  logout: () => void;
};

const AuthContext = createContext<AuthContextValue | undefined>(undefined);

type AuthProviderProps = {
  children: ReactNode;
};

export const AuthProvider = ({ children }: AuthProviderProps) => {
  const [user, setUser] = useState<User | null>(null);
  const [token, setToken] = useState<string | null>(() => getStoredToken());

  const {
    data,
    error,
    loading: isLoading,
  } = useQuery(ME, {
    skip: !token,
    fetchPolicy: 'network-only',
  });

  useEffect(() => {
    if (!token) {
      setUser(null);
      return;
    }

    const me = (data as any)?.me as User | null | undefined;
    if (me) {
      setUser(me);
      return;
    }
    
    if (error || (data && !me)) {
      setUser(null);
      setToken(null);
      removeStoredToken();
    }
  }, [data, error, token]);

  const login = (user: User, token: string) => {
    setToken(token);
    setStoredToken(token);
    setUser(user);
  };

  const logout = () => {
    setUser(null);
    setToken(null);
    removeStoredToken();
  };

  const value: AuthContextValue = useMemo(
    () => ({
      user,
      token,
      isAuthenticated: !!token,
      isLoading: !!token && isLoading,
      login,
      logout,
    }),
    [user, token, isLoading],
  );

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
};

export const useAuth = (): AuthContextValue => {
  const ctx = useContext(AuthContext);
  if (!ctx) {
    throw new Error('useAuth must be used within an AuthProvider');
  }
  return ctx;
};
