namespace NoFreeLaunch.Api.Services;

using NoFreeLaunch.Api.Data.Entities;

public interface IFavoritesService
{
    Task AddFavoriteAsync(string launchId, string userId, CancellationToken cancellationToken = default);
    Task RemoveFavoriteAsync(string launchId, string userId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Favorite>> GetFavoritesForUserAsync(string userId, CancellationToken cancellationToken = default);
}
