namespace NoFreeLaunch.Api.GraphQL;

using NoFreeLaunch.Api.Services;
using NoFreeLaunch.Api.Data.Entities;

public class AppMutations
{
    public async Task<bool> AddFavoriteAsync(
        string launchId,
        string userId,
        [Service] IFavoritesService favorites,
        CancellationToken cancellationToken)
    {
        await favorites.AddFavoriteAsync(launchId, userId, cancellationToken);
        return true;
    }

    public async Task<bool> RemoveFavoriteAsync(
        string launchId,
        string userId,
        [Service] IFavoritesService favorites,
        CancellationToken cancellationToken)
    {
        await favorites.RemoveFavoriteAsync(launchId, userId, cancellationToken);
        return true;
    }
}