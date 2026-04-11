import './Launches.css';
import { useMutation, useQuery } from '@apollo/client/react';
import { GET_FAVORITES, GET_LAUNCHES } from '../graphql/queries';
import type { GetFavoritesData, GetLaunchesData } from '../types/graphql';
import { formatDateString } from '../util/date';
import { ADD_FAVORITE, REMOVE_FAVORITE } from '../graphql/mutations';
import { useAuth } from '../contexts/useAuth';

export default function Launches() {
  const { token } = useAuth();
  const { data, loading, error } = useQuery<GetLaunchesData>(GET_LAUNCHES);
  const { data: favoritesData, refetch: refetchFavorites } = useQuery<GetFavoritesData>(GET_FAVORITES, {
    skip: !token,
  });
  const [addFavorite] = useMutation(ADD_FAVORITE);
  const [removeFavorite] = useMutation(REMOVE_FAVORITE);
  const favorites = favoritesData?.favorites ?? [];
  const launches = data?.launches || [];

  const handleFavoriteChange = async (launchId: string, checked: boolean) => {
    if (!token) return;
    if (checked) {
      await addFavorite({ variables: { launchId } });
    } else {
      await removeFavorite({ variables: { launchId } });
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
                disabled={!token}
                title={token ? undefined : 'Sign in to save favorites'}
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
