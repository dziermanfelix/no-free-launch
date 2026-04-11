import { useCallback, useEffect, useMemo, useState } from 'react';
import type { ReactNode } from 'react';
import { useQuery } from '@apollo/client/react';
import type { User } from '../types/user';
import type { MeQueryData } from '../types/graphql';
import { getStoredToken, removeStoredToken, setStoredToken } from '../util/token';
import { ME } from '../graphql/queries';
import { AuthContext } from './auth-context';

type AuthProviderProps = {
  children: ReactNode;
};

export function AuthProvider({ children }: AuthProviderProps) {
  const [user, setUser] = useState<User | null>(null);
  const [token, setToken] = useState<string | null>(() => getStoredToken());

  const {
    data,
    error,
    loading: isLoading,
  } = useQuery<MeQueryData>(ME, {
    skip: !token,
    fetchPolicy: 'network-only',
  });

  useEffect(() => {
    if (!token) {
      queueMicrotask(() => setUser(null));
      return;
    }

    const me = data?.me;
    if (me) {
      queueMicrotask(() => setUser(me));
      return;
    }

    if (error || (data && !me)) {
      queueMicrotask(() => {
        setUser(null);
        setToken(null);
        removeStoredToken();
      });
    }
  }, [data, error, token]);

  const contextUser = token ? user : null;

  const login = useCallback((nextUser: User, nextToken: string) => {
    setToken(nextToken);
    setStoredToken(nextToken);
    setUser(nextUser);
  }, []);

  const logout = useCallback(() => {
    setUser(null);
    setToken(null);
    removeStoredToken();
  }, []);

  const value = useMemo(
    () => ({
      user: contextUser,
      token,
      isAuthenticated: !!token,
      isLoading: !!token && isLoading,
      login,
      logout,
    }),
    [contextUser, token, isLoading, login, logout],
  );

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
}
