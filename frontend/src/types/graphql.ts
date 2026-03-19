import type { Favorite } from './favorite';
import type { Launch } from './launch';

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
