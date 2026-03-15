using Microsoft.EntityFrameworkCore;
using NoFreeLaunch.Api.Data;
using NoFreeLaunch.Api.Data.Entities;
using NoFreeLaunch.Api.Services;

namespace NoFreeLaunch.Api.GraphQL;

public class AppQueries
{
    public async Task<IEnumerable<Launch>> GetLaunchesAsync(
        [Service] NoFreeLaunchDbContext context,
        CancellationToken cancellationToken)
        => await context.Launches.OrderBy(l => l.FlightNumber).ToListAsync(cancellationToken);

    public async Task<Launch?> GetLaunchAsync(
        string id,
        [Service] NoFreeLaunchDbContext context,
        CancellationToken cancellationToken)
        => await context.Launches.FirstOrDefaultAsync(l => l.Id == id, cancellationToken);

    public async Task<Launch?> GetLaunchByFlightNumberAsync(
        int flightNumber,
        [Service] NoFreeLaunchDbContext context,
        CancellationToken cancellationToken)
        => await context.Launches.FirstOrDefaultAsync(l => l.FlightNumber == flightNumber, cancellationToken);

    public async Task<IReadOnlyList<Favorite>> GetFavoritesAsync(
        int userId,
        [Service] IFavoritesService favorites,
        CancellationToken cancellationToken)
        => await favorites.GetFavoritesForUserAsync(userId, cancellationToken);

    public async Task<IReadOnlyList<User>> GetUsersAsync(
        [Service] IUsersService users,
        CancellationToken cancellationToken)
        => await users.GetUsersAsync(cancellationToken);

    public async Task<User?> GetUserByNameAsync(
        string userName,
        [Service] IUsersService users,
        CancellationToken cancellationToken)
        => await users.GetUserByNameAsync(userName, cancellationToken);

}