using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NoFreeLaunch.Api.Data;
using NoFreeLaunch.Api.Data.Entities;

namespace NoFreeLaunch.Api.Services;

public sealed class AuthService : IAuthService
{
    private readonly NoFreeLaunchDbContext _context;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly ITokenService _tokenService;

    public AuthService(
        NoFreeLaunchDbContext context,
        IPasswordHasher<User> passwordHasher,
        ITokenService tokenService)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
    }

    public async Task<(User User, string Token)> RegisterAsync(
        string userName,
        string password,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(userName))
            throw new InvalidOperationException("Username is required.");
        if (string.IsNullOrWhiteSpace(password))
            throw new InvalidOperationException("Password is required.");

        var normalizedUserName = userName.Trim();
        bool exists = await _context.Users.AnyAsync(u => u.UserName == normalizedUserName, cancellationToken);
        if (exists)
            throw new InvalidOperationException("A user with that name already exists.");

        var user = new User { UserName = normalizedUserName };
        user.PasswordHash = _passwordHasher.HashPassword(user, password);

        _context.Users.Add(user);
        await _context.SaveChangesAsync(cancellationToken);

        var token = _tokenService.CreateAccessToken(user);
        return (user, token);
    }

    public async Task<(User User, string Token)> LoginAsync(
        string userName,
        string password,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
            throw new InvalidOperationException("Invalid username or password.");

        var normalizedUserName = userName.Trim();
        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == normalizedUserName, cancellationToken);
        if (user is null)
            throw new InvalidOperationException("Invalid username or password.");

        if (string.IsNullOrWhiteSpace(user.PasswordHash))
            throw new InvalidOperationException("Password login is not enabled for this user.");

        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
        if (result == PasswordVerificationResult.Failed)
            throw new InvalidOperationException("Invalid username or password.");

        if (result == PasswordVerificationResult.SuccessRehashNeeded)
        {
            user.PasswordHash = _passwordHasher.HashPassword(user, password);
            await _context.SaveChangesAsync(cancellationToken);
        }

        var token = _tokenService.CreateAccessToken(user);
        return (user, token);
    }
}

