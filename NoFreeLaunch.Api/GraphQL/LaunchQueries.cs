using NoFreeLaunch.Api.Clients;

namespace NoFreeLaunch.Api.GraphQL;

public class LaunchQueries
{
    public async Task<IEnumerable<SpaceXLaunchDto>> GetLaunchesAsync(
        [Service] ISpaceXLaunchClient client,
        CancellationToken cancellationToken)
        => await client.GetLaunchesAsync(cancellationToken);

    public async Task<SpaceXLaunchDto?> GetLaunchAsync(
        string id,
        [Service] ISpaceXLaunchClient client,
        CancellationToken cancellationToken)
        => await client.GetLaunchByIdAsync(id, cancellationToken);

    public async Task<SpaceXLaunchDto?> GetLaunchByFlightNumberAsync(
    int flightNumber,
    [Service] ISpaceXLaunchClient client,
    CancellationToken cancellationToken)
    => await client.GetLaunchByFlightNumberAsync(flightNumber, cancellationToken);
}