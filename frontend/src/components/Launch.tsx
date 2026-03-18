import { useQuery } from '@apollo/client/react';
import { GET_LAUNCH_BY_FLIGHT_NUMBER, GET_LAUNCH_BY_ID } from '../graphql/queries';
import type { GetLaunchData } from '../types/graphql';

interface LaunchProps {
  id: string | null;
  flightNumber: number | null;
}

export default function Launch({ id = null, flightNumber = null }: LaunchProps) {
  const queryById = id !== null;

  const {
    data: dataById,
    loading: loadingById,
    error: errorById,
  } = useQuery<GetLaunchData>(GET_LAUNCH_BY_ID, {
    variables: { id },
    skip: !queryById,
  });

  const {
    data: dataByFlightNumber,
    loading: loadingByFlightNumber,
    error: errorByFlightNumber,
  } = useQuery<GetLaunchData>(GET_LAUNCH_BY_FLIGHT_NUMBER, {
    variables: { flightNumber },
    skip: queryById,
  });

  const loading = id ? loadingById : loadingByFlightNumber;
  const error = id ? errorById : errorByFlightNumber;

  if (loading) return <div>Loading...</div>;
  if (error) return <div>Error: {error.message}</div>;

  const launch = id ? dataById?.launch : dataByFlightNumber?.launchByFlightNumber;

  return (
    <div>
      <h1>Launch</h1>
      <h2>{launch?.name}</h2>
      <h3>{launch?.flightNumber}</h3>
      <p>{launch?.dateUtc}</p>
    </div>
  );
}
