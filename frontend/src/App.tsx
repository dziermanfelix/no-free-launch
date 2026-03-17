import './App.css';
import { useQuery } from '@apollo/client/react';
import { GET_LAUNCHES } from './graphql/queries';
import type { Launch } from './types/launch';

type GetLaunchesData = {
  launches: Launch[];
};

function App() {
  const { data, loading, error } = useQuery<GetLaunchesData>(GET_LAUNCHES);

  if (loading) return <div>Loading...</div>;
  if (error) return <div>Error: {error.message}</div>;

  const launches = data?.launches || [];

  return (
    <>
      <section id='center'>
        <div className='hero'>
          <h1>No Free Launch</h1>
        </div>
        <div>
          {launches.map((launch) => (
            <div key={launch.id}>
              <h2>{launch.name}</h2>
              <h3>{launch.flightNumber}</h3>
              <p>{launch.dateUtc}</p>
            </div>
          ))}
        </div>
      </section>
    </>
  );
}

export default App;
