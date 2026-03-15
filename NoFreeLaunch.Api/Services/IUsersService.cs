namespace NoFreeLaunch.Api.Services;

using NoFreeLaunch.Api.Data.Entities;

public interface IUsersService
{
    Task CreateUserAsync(string userName, CancellationToken cancellationToken);
    Task DeleteUserAsync(string userName, CancellationToken cancellationToken);
    Task<IReadOnlyList<User>> GetUsersAsync(CancellationToken cancellationToken);
}