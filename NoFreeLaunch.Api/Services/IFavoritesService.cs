using NoFreeLaunch.Api.Data.Entities;

namespace NoFreeLaunch.Api.Services;

public interface IFavoritesService
{
    Task AddFavoriteAsync(string launchId, int userId, CancellationToken cancellationToken = default);
    Task RemoveFavoriteAsync(string launchId, int userId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Favorite>> GetFavoritesForUserAsync(int userId, CancellationToken cancellationToken = default);
}
