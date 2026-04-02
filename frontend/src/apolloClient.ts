import { ApolloClient, InMemoryCache, HttpLink, ApolloLink } from '@apollo/client';
import { getStoredToken } from './util/token';

const httpLink = new HttpLink({
  uri: 'http://localhost:5021/graphql',
});

const authLink = new ApolloLink((operation, forward) => {
  const token = getStoredToken();

  operation.setContext(({ headers = {} }: { headers?: Record<string, string> }) => ({
    headers: {
      ...headers,
      ...(token ? { Authorization: `Bearer ${token}` } : {}),
    },
  }));

  return forward(operation);
});

export const apolloClient = new ApolloClient({
  link: authLink.concat(httpLink),
  cache: new InMemoryCache(),
});
