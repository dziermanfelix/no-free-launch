import './Launches.css';
import { useQuery } from '@apollo/client/react';
import { GET_FAVORITES, GET_LAUNCHES } from '../graphql/queries';
import type { GetFavoritesData, GetLaunchesData } from '../types/graphql';
import { formatDateString } from '../util/date';

export default function Launches() {
  const { data, loading, error } = useQuery<GetLaunchesData>(GET_LAUNCHES);
  // TODO get current user
  const userId = 1;
  const { data: favoritesData } = useQuery<GetFavoritesData>(GET_FAVORITES, {
    variables: { userId },
  });
  const favorites = favoritesData?.favorites ?? [];
  const launches = data?.launches || [];

  const handleFavoriteClick = (launchId: string) => {
    console.log(`Favorite clicked for launch: ${launchId}`);
  };

  if (loading) return <div>Loading...</div>;
  if (error) return <div>Error: {error.message}</div>;

  return (
    <div className='launches'>
      <ul className='launches-list'>
        {launches.map((launch) => (
          <li key={launch.id} className='launches-row'>
            <div className='launches-row-inner'>
              <input
                className='launches-fav'
                type='checkbox'
                checked={favorites.some((f) => f.launchId === launch.id)}
                onChange={() => handleFavoriteClick(launch.id)}
              />
              <div className='launches-details'>
                <span>{launch.flightNumber ?? 'Unknown Flight Number'}</span>
                <span>{launch.name ?? 'Unknown Name'}</span>
              </div>
              <span>{launch.dateUtc ? formatDateString(launch.dateUtc) : 'Unknown Date'}</span>
            </div>
          </li>
        ))}
      </ul>
    </div>
  );
}
