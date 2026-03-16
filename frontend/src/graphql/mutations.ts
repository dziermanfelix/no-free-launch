import { gql } from '@apollo/client';

export const CREATE_USER = gql`
  mutation CreateUser($userName: String!) {
    createUser(userName: $userName) {
      id
      userName
    }
  }
`;

export const DELETE_USER = gql`
  mutation DeleteUser($userName: String!) {
    deleteUser(userName: $userName)
  }
`;

export const ADD_FAVORITE = gql`
  mutation AddFavorite($launchId: String!, $userId: Int!) {
    addFavorite(launchId: $launchId, userId: $userId)
  }
`;

export const REMOVE_FAVORITE = gql`
  mutation RemoveFavorite($launchId: String!, $userId: Int!) {
    removeFavorite(launchId: $launchId, userId: $userId)
  }
`;
