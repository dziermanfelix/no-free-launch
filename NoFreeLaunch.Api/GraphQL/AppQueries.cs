using Microsoft.EntityFrameworkCore;
using NoFreeLaunch.Api.Data;
using NoFreeLaunch.Api.Data.Entities;
using NoFreeLaunch.Api.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace NoFreeLaunch.Api.GraphQL;

public class AppQueries
{
    [AllowAnonymous]
    public async Task<IEnumerable<Launch>> GetLaunchesAsync(
        [Service] NoFreeLaunchDbContext context,
        CancellationToken cancellationToken)
        => await context.Launches.OrderBy(l => l.FlightNumber).ToListAsync(cancellationToken);

    [AllowAnonymous]
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

    [Authorize]
    public async Task<IReadOnlyList<Favorite>> GetFavoritesAsync(
        ClaimsPrincipal user,
        [Service] IFavoritesService favorites,
        CancellationToken cancellationToken)
    {
        var userId = int.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)!);
        return await favorites.GetFavoritesForUserAsync(userId, cancellationToken);
    }

    [Authorize]
    public async Task<IReadOnlyList<User>> GetUsersAsync(
        [Service] IUsersService users,
        CancellationToken cancellationToken)
        => await users.GetUsersAsync(cancellationToken);

    [Authorize]
    public async Task<User?> GetUserByNameAsync(
        string userName,
        [Service] IUsersService users,
        CancellationToken cancellationToken)
        => await users.GetUserByNameAsync(userName, cancellationToken);

    [Authorize]
    public async Task<User?> GetMeAsync(
        ClaimsPrincipal user,
        [Service] NoFreeLaunchDbContext context,
        CancellationToken cancellationToken)
    {
        var userIdValue = user.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(userIdValue, out var userId))
            return null;

        return await context.Users.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
    }

}