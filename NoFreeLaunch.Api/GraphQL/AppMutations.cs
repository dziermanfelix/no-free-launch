namespace NoFreeLaunch.Api.GraphQL;

using NoFreeLaunch.Api.Services;
using NoFreeLaunch.Api.Data.Entities;

public class AppMutations
{
    public async Task<User> CreateUserAsync(
        string userName,
        [Service] IUsersService users,
        CancellationToken cancellationToken)
    {
        return await users.CreateUserAsync(userName, cancellationToken);
    }

    public async Task<bool> DeleteUserAsync(
        string userName,
        [Service] IUsersService users,
        CancellationToken cancellationToken)
    {
        await users.DeleteUserAsync(userName, cancellationToken);
        return true;
    }

    public async Task<bool> AddFavoriteAsync(
        string launchId,
        int userId,
        [Service] IFavoritesService favorites,
        CancellationToken cancellationToken)
    {
        await favorites.AddFavoriteAsync(launchId, userId, cancellationToken);
        return true;
    }

    public async Task<bool> RemoveFavoriteAsync(
        string launchId,
        int userId,
        [Service] IFavoritesService favorites,
        CancellationToken cancellationToken)
    {
        await favorites.RemoveFavoriteAsync(launchId, userId, cancellationToken);
        return true;
    }

    public async Task<AuthPayload> RegisterAsync(
        string userName,
        string password,
        [Service] IAuthService auth,
        CancellationToken cancellationToken)
    {
        var (user, token) = await auth.RegisterAsync(userName, password, cancellationToken);
        return new AuthPayload { User = user, Token = token };
    }

    public async Task<AuthPayload> LoginAsync(
        string userName,
        string password,
        [Service] IAuthService auth,
        CancellationToken cancellationToken)
    {
        var (user, token) = await auth.LoginAsync(userName, password, cancellationToken);
        return new AuthPayload { User = user, Token = token };
    }
}