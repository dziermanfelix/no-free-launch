using Microsoft.AspNetCore.Mvc;
using NoFreeLaunch.Api.Services;

namespace NoFreeLaunch.Api.Controllers;

[ApiController]
[Route("spacex/launches")]
public class SpaceXLaunchesController(ILaunchesService launches) : ControllerBase
{
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
