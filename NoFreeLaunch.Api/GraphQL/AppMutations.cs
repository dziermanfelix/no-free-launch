using NoFreeLaunch.Api.Services;
using System.Security.Claims;
using HotChocolate.Authorization;
using HotChocolate.AspNetCore.Authorization;

namespace NoFreeLaunch.Api.GraphQL;

public class AppMutations
{
    [Authorize]
    public async Task<bool> DeleteUserAsync(
            string userName,
            [Service] IUsersService users,
            CancellationToken cancellationToken)
    {
        await users.DeleteUserAsync(userName, cancellationToken);
        return true;
    }

    [Authorize]
    public async Task<bool> AddFavoriteAsync(
        string launchId,
        ClaimsPrincipal user,
        [Service] IFavoritesService favorites,
        CancellationToken cancellationToken)
    {
        var userId = int.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)!);
        await favorites.AddFavoriteAsync(launchId, userId, cancellationToken);
        return true;
    }

    [Authorize]
    public async Task<bool> RemoveFavoriteAsync(
        string launchId,
        ClaimsPrincipal user,
        [Service] IFavoritesService favorites,
        CancellationToken cancellationToken)
    {
        var userId = int.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)!);
        await favorites.RemoveFavoriteAsync(launchId, userId, cancellationToken);
        return true;
    }

    [AllowAnonymous]
    public async Task<AuthPayload> RegisterAsync(
        string userName,
        string password,
        [Service] IAuthService auth,
        CancellationToken cancellationToken)
    {
        var (user, token) = await auth.RegisterAsync(userName, password, cancellationToken);
        return new AuthPayload { User = user, Token = token };
    }

    [AllowAnonymous]
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