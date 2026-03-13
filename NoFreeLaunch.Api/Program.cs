using NoFreeLaunch.Api.Clients;
using NoFreeLaunch.Api.GraphQL;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddHttpClient<ISpaceXLaunchClient, SpaceXLaunchClient>(client =>
{
    client.BaseAddress = new Uri("https://api.spacexdata.com/");
});

builder.Services.AddGraphQLServer().AddQueryType<LaunchQueries>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGraphQL();

app.MapGet("/spacex/launches", async (ISpaceXLaunchClient client, CancellationToken ct) =>
{
    var launches = await client.GetLaunchesAsync(ct);
    return Results.Ok(launches);
});

app.MapGet("/spacex/launches/{id}", async (string id, ISpaceXLaunchClient client, CancellationToken ct) =>
{
    var launch = await client.GetLaunchByIdAsync(id, ct);
    return launch is not null ? Results.Ok(launch) : Results.NotFound();
});

app.MapGet("/spacex/launches/number/{number}", async (int number, ISpaceXLaunchClient client, CancellationToken ct) =>
{
    var launch = await client.GetLaunchByFlightNumberAsync(number, ct);
    return launch is not null ? Results.Ok(launch) : Results.NotFound();
});

app.Run();
