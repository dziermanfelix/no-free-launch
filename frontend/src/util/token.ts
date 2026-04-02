import type { User } from "../types/user";

const TOKEN_STORAGE_KEY = 'nfl_token';
const USER_STORAGE_KEY = 'nfl_user';

export const getStoredToken = () => {
  return localStorage.getItem(TOKEN_STORAGE_KEY);
};

export const setStoredToken = (token: string) => {
  localStorage.setItem(TOKEN_STORAGE_KEY, token);
};

export const removeStoredToken = () => {
  localStorage.removeItem(TOKEN_STORAGE_KEY);
};
export const getStoredUser = () => {
  return localStorage.getItem(USER_STORAGE_KEY);
};

export const setStoredUser = (user: User) => {
  localStorage.setItem(USER_STORAGE_KEY, JSON.stringify(user));
};

export const removeStoredUser = () => {
  localStorage.removeItem(USER_STORAGE_KEY);
};
