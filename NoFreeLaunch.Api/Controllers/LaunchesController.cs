using Microsoft.AspNetCore.Mvc;
using NoFreeLaunch.Api.Clients;
using NoFreeLaunch.Api.Services;

namespace NoFreeLaunch.Api.Controllers;

[ApiController]
[Route("launches")]
public class LaunchesController(ILaunchesService launches) : ControllerBase
{
    [HttpPost("sync")]
    public async Task<IResult> SyncAsync(
        [FromServices] ISpaceXLaunchClient spaceXClient,
        CancellationToken cancellationToken)
    {
        var result = await launches.LaunchesSyncAsync(spaceXClient, cancellationToken);
        return Results.Ok(result);
    }

    [HttpGet]
    public async Task<IResult> GetAllAsync(CancellationToken cancellationToken)
    {
        var list = await launches.GetLaunchesAsync(cancellationToken);
        return Results.Ok(list);
    }

    [HttpGet("{id}")]
    public async Task<IResult> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        var launch = await launches.GetLaunchAsync(id, cancellationToken);
        return launch is not null ? Results.Ok(launch) : Results.NotFound();
    }

    [HttpGet("number/{number:int}")]
    public async Task<IResult> GetByFlightNumberAsync(int number, CancellationToken cancellationToken)
    {
        var launch = await launches.GetLaunchByFlightNumberAsync(number, cancellationToken);
        return launch is not null ? Results.Ok(launch) : Results.NotFound();
    }
}
