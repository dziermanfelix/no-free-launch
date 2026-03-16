import { gql } from '@apollo/client';

export const GET_LAUNCHES = gql`
  query GetLaunches {
    launches {
      id
      flightNumber
      name
      dateUtc
    }
  }
`;

export const GET_LAUNCH_BY_ID = gql`
  query GetLaunch($id: ID!) {
    launch(id: $id) {
      id
      flightNumber
      name
      dateUtc
    }
  }
`;

export const GET_LAUNCH_BY_FLIGHT_NUMBER = gql`
  query GetLaunch($flightNumber: Int!) {
    launch(flightNumber: $flightNumber) {
      id
      flightNumber
      name
      dateUtc
    }
  }
`;

export const GET_FAVORITES = gql`
  query GetFavorites($userId: Int!) {
    favorites(userId: $userId) {
      id
      launchId
      userId
      createdAt
    }
  }
`;

export const GET_USERS = gql`
  query GetUsers {
    users {
      id
      userName
    }
  }
`;

export const GET_USER_BY_NAME = gql`
  query GetUserByName($userName: String!) {
    user(userName: $userName) {
      id
      userName
    }
  }
`;
