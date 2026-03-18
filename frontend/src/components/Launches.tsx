import { useQuery } from '@apollo/client/react';
import { GET_LAUNCHES } from '../graphql/queries';
import type { GetLaunchesData } from '../types/graphql';

export default function Launches() {
  const { data, loading, error } = useQuery<GetLaunchesData>(GET_LAUNCHES);
  const launches = data?.launches || [];

  if (loading) return <div>Loading...</div>;
  if (error) return <div>Error: {error.message}</div>;

  return (
    <div>
      <h1>Launches</h1>
      <ul>
        {launches.map((launch) => (
          <li key={launch.id}>{launch.name}</li>
        ))}
      </ul>
    </div>
  );
}
