const TOKEN_STORAGE_KEY = 'nfl_token';

export const getStoredToken = () => {
  return localStorage.getItem(TOKEN_STORAGE_KEY);
};

export const setStoredToken = (token: string) => {
  localStorage.setItem(TOKEN_STORAGE_KEY, token);
};

export const removeStoredToken = () => {
  localStorage.removeItem(TOKEN_STORAGE_KEY);
};
