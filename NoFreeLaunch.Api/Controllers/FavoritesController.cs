using Microsoft.AspNetCore.Mvc;
using NoFreeLaunch.Api.Services;

namespace NoFreeLaunch.Api.Controllers;

[ApiController]
[Route("favorites")]
public class FavoritesController(IFavoritesService favorites) : ControllerBase
{
    [HttpGet("{userId:int}")]
    public async Task<IResult> GetForUserAsync(int userId, CancellationToken cancellationToken)
    {
        var list = await favorites.GetFavoritesForUserAsync(userId, cancellationToken);
        return Results.Ok(list);
    }
}
