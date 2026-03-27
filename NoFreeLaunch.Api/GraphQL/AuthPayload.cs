using NoFreeLaunch.Api.Data.Entities;

namespace NoFreeLaunch.Api.GraphQL;

public sealed class AuthPayload
{
    public User User { get; init; } = null!;
    public string Token { get; init; } = null!;
}