import React, { createContext, useContext, useState, useEffect } from 'react';
import type { ReactNode } from 'react';
import type { User } from '../types/user';
import { useQuery } from '@apollo/client/react';
import {
  getStoredToken,
  getStoredUser,
  removeStoredToken,
  removeStoredUser,
  setStoredToken,
  setStoredUser,
} from '../util/token';
import { ME } from '../graphql/queries';

type AuthContextValue = {
  user: User | null;
  token: string | null;
  isAuthenticated: boolean;
  login: (user: User, token: string) => void;
  logout: () => void;
};

const AuthContext = createContext<AuthContextValue | undefined>(undefined);

type AuthProviderProps = {
  children: ReactNode;
};

export const AuthProvider: React.FC<AuthProviderProps> = ({ children }) => {
  const [user, setUser] = useState<User | null>(null);
  const [token, setToken] = useState<string | null>(null);
  const { data, error } = useQuery<{ me: User | null }>(ME, {
    skip: !token,
    fetchPolicy: 'network-only',
  });

  useEffect(() => {
    const storedToken = getStoredToken();
    const storedUser = getStoredUser();

    if (!storedToken) return;

    setToken(storedToken);

    if (storedUser) {
      try {
        const parsedUser: User = JSON.parse(storedUser);
        setUser(parsedUser);
      } catch {
        removeStoredUser();
      }
    }
  }, []);

  useEffect(() => {
    if (!token) return;

    if (error || (data && !data.me)) {
      setUser(null);
      setToken(null);
      removeStoredToken();
      removeStoredUser();
      return;
    }

    if (data?.me) {
      setUser(data.me);
      setStoredUser(data.me);
    }
  }, [token, data, error]);

  const login = (user: User, token: string) => {
    setUser(user);
    setToken(token);
    setStoredToken(token);
    setStoredUser(user);
  };

  const logout = () => {
    setUser(null);
    setToken(null);
    removeStoredToken();
    removeStoredUser();
  };

  const value: AuthContextValue = {
    user,
    token,
    isAuthenticated: !!token,
    login,
    logout,
  };

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
};

export const useAuth = (): AuthContextValue => {
  const ctx = useContext(AuthContext);
  if (!ctx) {
    throw new Error('useAuth must be used within an AuthProvider');
  }
  return ctx;
};
