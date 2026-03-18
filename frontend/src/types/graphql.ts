import type { Launch } from './launch';

export type GetLaunchesData = {
  launches: Launch[];
};

export type GetLaunchData = {
  launch: Launch | null;
  launchByFlightNumber: Launch | null;
};
