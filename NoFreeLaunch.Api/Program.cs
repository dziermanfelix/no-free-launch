using NoFreeLaunch.Api.Clients;
using NoFreeLaunch.Api.GraphQL;
using Microsoft.EntityFrameworkCore;
using NoFreeLaunch.Api.Data;
using NoFreeLaunch.Api.Data.Entities;
using NoFreeLaunch.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<NoFreeLaunchDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IFavoritesService, FavoritesService>();
builder.Services.AddScoped<ILaunchesService, LaunchesService>();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddHttpClient<ISpaceXLaunchClient, SpaceXLaunchClient>(client =>
{
    client.BaseAddress = new Uri("https://api.spacexdata.com/");
});

builder.Services
.AddGraphQLServer()
.AddQueryType<AppQueries>()
.AddMutationType<AppMutations>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGraphQL();

app.MapPost("launches/sync", async (ISpaceXLaunchClient client, ILaunchesService launches, CancellationToken ct) =>
{
    var result = await launches.LaunchesSyncAsync(client, ct);
    return Results.Ok(result);
});

app.MapGet("/spacex/launches", async (ILaunchesService launches, CancellationToken ct) =>
{
    var list = await launches.GetLaunchesAsync(ct);
    return Results.Ok(list);
});

app.MapGet("/spacex/launches/{id}", async (string id, ILaunchesService launches, CancellationToken ct) =>
{
    var launch = await launches.GetLaunchAsync(id, ct);
    return launch is not null ? Results.Ok(launch) : Results.NotFound();
});

app.MapGet("/spacex/launches/number/{number}", async (int number, ILaunchesService launches, CancellationToken ct) =>
{
    var launch = await launches.GetLaunchByFlightNumberAsync(number, ct);
    return launch is not null ? Results.Ok(launch) : Results.NotFound();
});

app.MapGet("/favorites/{userId}", async (string userId, IFavoritesService favorites, CancellationToken ct) =>
{
    var list = await favorites.GetFavoritesForUserAsync(userId, ct);
    return Results.Ok(list);
});

app.Run();
