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

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddHttpClient<ISpaceXLaunchClient, SpaceXLaunchClient>(client =>
{
    client.BaseAddress = new Uri("https://api.spacexdata.com/");
});

builder.Services
.AddGraphQLServer()
.AddQueryType<LaunchQueries>()
.AddMutationType<FavoritesMutations>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGraphQL();

app.MapPost("launches/sync", async (ISpaceXLaunchClient client, NoFreeLaunchDbContext context, CancellationToken ct) =>
{
    var fromApi = await client.GetLaunchesAsync(ct);
    var existingIds = await context.Launches.Select(l => l.Id).ToListAsync(ct);
    var toInsert = fromApi.Where(dto => !string.IsNullOrEmpty(dto.Id) && !existingIds.Contains(dto.Id)).Select(dto => new Launch
    {
        Id = dto.Id!,
        Name = dto.Name,
        FlightNumber = dto.FlightNumber,
        DateUtc = dto.DateUtc,
        FetchedAt = DateTime.UtcNow
    }).ToList();
    context.Launches.AddRange(toInsert);
    await context.SaveChangesAsync(ct);
    return Results.Ok(new { added = toInsert.Count, total = await context.Launches.CountAsync(ct) });
});

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

app.MapGet("/favorites/{userId}", async (string userId, IFavoritesService favorites, CancellationToken ct) =>
{
    var list = await favorites.GetFavoritesForUserAsync(userId, ct);
    return Results.Ok(list);
});

app.Run();
