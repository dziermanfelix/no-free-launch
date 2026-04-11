import type { Favorite } from './favorite';
import type { Launch } from './launch';
import type { User } from './user';

export type MeQueryData = {
  me: User | null;
};

export type LoginMutationData = {
  login: {
    user: User;
    token: string;
  };
};

export type RegisterMutationData = {
  register: {
    user: User;
    token: string;
  };
};

export type GetLaunchesData = {
  launches: Launch[];
};

export type GetLaunchData = {
  launch: Launch | null;
  launchByFlightNumber: Launch | null;
};

export type GetFavoritesData = {
  favorites: Favorite[];
};
