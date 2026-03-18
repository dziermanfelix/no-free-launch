import { useQuery } from '@apollo/client/react';
import { GET_LAUNCH_BY_FLIGHT_NUMBER, GET_LAUNCH_BY_ID } from '../graphql/queries';
import type { GetLaunchData } from '../types/graphql';
import { useParams } from 'react-router-dom';

export default function Launch() {
  const { id, flightNumber } = useParams<{
    id?: string;
    flightNumber?: string;
  }>();

  const queryById = id !== undefined;
  const flightNumberValue = flightNumber ? parseInt(flightNumber) : null;

  const {
    data: dataById,
    loading: loadingById,
    error: errorById,
  } = useQuery<GetLaunchData>(GET_LAUNCH_BY_ID, {
    variables: { id: id ?? '' },
    skip: !queryById,
  });

  const {
    data: dataByFlightNumber,
    loading: loadingByFlightNumber,
    error: errorByFlightNumber,
  } = useQuery<GetLaunchData>(GET_LAUNCH_BY_FLIGHT_NUMBER, {
    variables: { flightNumber: flightNumberValue ?? 0 },
    skip: queryById,
  });

  const loading = queryById ? loadingById : loadingByFlightNumber;
  const error = queryById ? errorById : errorByFlightNumber;

  if (loading) return <div>Loading...</div>;
  if (error) return <div>Error: {error.message}</div>;

  const launch = queryById ? dataById?.launch : dataByFlightNumber?.launchByFlightNumber;

  return (
    <div>
      <h1>Launch</h1>
      <h2>{launch?.name}</h2>
      <h3>{launch?.flightNumber}</h3>
      <p>{launch?.dateUtc}</p>
    </div>
  );
}
