using NoFreeLaunch.Api.Clients;
using NoFreeLaunch.Api.Data.Entities;
using NoFreeLaunch.Api.Models;
using NoFreeLaunch.Api.Services;

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

    public async Task<IReadOnlyList<Favorite>> GetFavoritesAsync(
        string userId,
        [Service] IFavoritesService favorites,
        CancellationToken cancellationToken)
        => await favorites.GetFavoritesForUserAsync(userId, cancellationToken);
}