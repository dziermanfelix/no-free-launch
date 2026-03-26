using NoFreeLaunch.Api.Data.Entities;

namespace NoFreeLaunch.Api.Services;

public interface ITokenService
{
    string CreateAccessToken(User user);
}

