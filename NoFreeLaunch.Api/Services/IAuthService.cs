using NoFreeLaunch.Api.Data.Entities;

namespace NoFreeLaunch.Api.Services;

public interface IAuthService
{
    Task<(User User, string Token)> RegisterAsync(string userName, string password, CancellationToken cancellationToken = default);
    Task<(User User, string Token)> LoginAsync(string userName, string password, CancellationToken cancellationToken = default);
}

