import { gql } from '@apollo/client';

export const REGISTER = gql`
  mutation Register($username: String!, $password: String!) {
    register(userName: $username, password: $password) {
      user {
        id
        userName
      }
      token
    }
  }
`;

export const LOGIN = gql`
  mutation Login($username: String!, $password: String!) {
    login(userName: $username, password: $password) {
      user {
        id
        userName
      }
      token
    }
  }
`;

export const DELETE_USER = gql`
  mutation DeleteUser($userName: String!) {
    deleteUser(userName: $userName)
  }
`;

export const ADD_FAVORITE = gql`
  mutation AddFavorite($launchId: String!) {
    addFavorite(launchId: $launchId)
  }
`;

export const REMOVE_FAVORITE = gql`
  mutation RemoveFavorite($launchId: String!) {
    removeFavorite(launchId: $launchId)
  }
`;
