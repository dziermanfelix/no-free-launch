using Microsoft.EntityFrameworkCore;
using NoFreeLaunch.Api.Data;
using NoFreeLaunch.Api.Data.Entities;

namespace NoFreeLaunch.Api.Services;

public class UsersService : IUsersService
{
    private readonly NoFreeLaunchDbContext _context;

    public UsersService(NoFreeLaunchDbContext context)
    {
        _context = context;
    }

    public async Task CreateUserAsync(string userName, CancellationToken cancellationToken = default)
    {
        bool exists = await _context.Users.AnyAsync(u => u.UserName == userName, cancellationToken);
        if (exists)
            throw new InvalidOperationException("A user with that name already exists.");
        _context.Users.Add(new User { UserName = userName });
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteUserAsync(string userName, CancellationToken cancellationToken = default)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName, cancellationToken);
        if (user is not null)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<IReadOnlyList<User>> GetUsersAsync(CancellationToken cancellationToken)
    {
        return await _context.Users.OrderBy(l => l.UserName).ToListAsync(cancellationToken);
    }

    public async Task<User?> GetUserByNameAsync(string userName, CancellationToken cancellationToken)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName, cancellationToken);
    }
}
