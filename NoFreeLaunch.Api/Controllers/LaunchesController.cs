using Microsoft.AspNetCore.Mvc;
using NoFreeLaunch.Api.Clients;
using NoFreeLaunch.Api.Services;

namespace NoFreeLaunch.Api.Controllers;

[ApiController]
[Route("launches")]
public class LaunchesController(ISpaceXLaunchClient spaceXClient, ILaunchesService launches) : ControllerBase
{
    [HttpPost("sync")]
    public async Task<IResult> SyncAsync(CancellationToken cancellationToken)
    {
        var result = await launches.LaunchesSyncAsync(spaceXClient, cancellationToken);
        return Results.Ok(result);
    }
}
