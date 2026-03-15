namespace NoFreeLaunch.Api.Services;

using NoFreeLaunch.Api.Data.Entities;

public interface IUsersService
{
    Task<User> CreateUserAsync(string userName, CancellationToken cancellationToken);
    Task DeleteUserAsync(string userName, CancellationToken cancellationToken);
    Task<IReadOnlyList<User>> GetUsersAsync(CancellationToken cancellationToken);
    Task<User?> GetUserByNameAsync(string userName, CancellationToken cancellationToken);
}