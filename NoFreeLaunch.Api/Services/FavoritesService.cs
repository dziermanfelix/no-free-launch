using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NoFreeLaunch.Api.Data;
using NoFreeLaunch.Api.Data.Entities;

namespace NoFreeLaunch.Api.Services;

public class FavoritesService : IFavoritesService
{
    private readonly NoFreeLaunchDbContext _context;

    public FavoritesService(NoFreeLaunchDbContext context)
    {
        _context = context;
    }

    public async Task AddFavoriteAsync(string launchId, string userId, CancellationToken cancellationToken = default)
    {
        await _context.Database.ExecuteSqlRawAsync(
            "EXEC dbo.AddFavorite @LaunchId, @UserId",
            [new SqlParameter("@LaunchId", launchId), new SqlParameter("@UserId", userId)],
            cancellationToken);
    }

    public async Task RemoveFavoriteAsync(string launchId, string userId, CancellationToken cancellationToken = default)
    {
        await _context.Database.ExecuteSqlRawAsync(
            "EXEC dbo.RemoveFavorite @LaunchId, @UserId",
            [new SqlParameter("@LaunchId", launchId), new SqlParameter("@UserId", userId)],
            cancellationToken);
    }

    public async Task<IReadOnlyList<Favorite>> GetFavoritesForUserAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await _context.Favorites
            .Where(f => f.UserId == userId)
            .Include(f => f.Launch)
            .OrderByDescending(f => f.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}
