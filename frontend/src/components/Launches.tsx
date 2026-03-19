import './Launches.css';
import { useMutation, useQuery } from '@apollo/client/react';
import { GET_FAVORITES, GET_LAUNCHES } from '../graphql/queries';
import type { GetFavoritesData, GetLaunchesData } from '../types/graphql';
import { formatDateString } from '../util/date';
import { ADD_FAVORITE, REMOVE_FAVORITE } from '../graphql/mutations';

export default function Launches() {
  const { data, loading, error } = useQuery<GetLaunchesData>(GET_LAUNCHES);
  // TODO get current user from context
  const userId = 1;
  const { data: favoritesData, refetch: refetchFavorites } = useQuery<GetFavoritesData>(GET_FAVORITES, {
    variables: { userId },
  });
  const [addFavorite] = useMutation(ADD_FAVORITE);
  const [removeFavorite] = useMutation(REMOVE_FAVORITE);
  const favorites = favoritesData?.favorites ?? [];
  const launches = data?.launches || [];

  const handleFavoriteChange = async (launchId: string, checked: boolean) => {
    if (checked) {
      await addFavorite({ variables: { launchId, userId } });
    } else {
      await removeFavorite({ variables: { launchId, userId } });
    }
    void refetchFavorites();
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
                onChange={(e) => void handleFavoriteChange(launch.id, e.target.checked)}
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
