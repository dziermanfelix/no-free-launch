namespace NoFreeLaunch.Api.Services;

public sealed class JwtOptions
{
    public string Issuer { get; init; } = null!;
    public string Audience { get; init; } = null!;
    public int ExpiresMinutes { get; init; } = 60;
}

