import { createContext } from 'react';
import type { User } from '../types/user';

export type AuthContextValue = {
  user: User | null;
  token: string | null;
  isAuthenticated: boolean;
  isLoading: boolean;
  login: (user: User, token: string) => void;
  logout: () => void;
};

export const AuthContext = createContext<AuthContextValue | undefined>(undefined);
