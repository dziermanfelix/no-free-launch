namespace NoFreeLaunch.Api.Services;

using NoFreeLaunch.Api.Data.Entities;
using NoFreeLaunch.Api.Data;
using Microsoft.EntityFrameworkCore;
using NoFreeLaunch.Api.Clients;

public class LaunchesService : ILaunchesService
{
    private readonly NoFreeLaunchDbContext _context;

    public LaunchesService(NoFreeLaunchDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Launch>> GetLaunchesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Launches.OrderBy(l => l.FlightNumber).ToListAsync(cancellationToken);
    }

    public async Task<Launch?> GetLaunchAsync(string id, CancellationToken cancellationToken = default)
    {
        return await _context.Launches.FirstOrDefaultAsync(l => l.Id == id, cancellationToken);
    }

    public async Task<Launch?> GetLaunchByFlightNumberAsync(int flightNumber, CancellationToken cancellationToken = default)
    {
        return await _context.Launches.FirstOrDefaultAsync(l => l.FlightNumber == flightNumber, cancellationToken);
    }

    public async Task<LaunchesSyncResult> LaunchesSyncAsync(ISpaceXLaunchClient client, CancellationToken cancellationToken = default)
    {
        var fromApi = await client.GetLaunchesAsync(cancellationToken);
        var existingIds = await _context.Launches.Select(l => l.Id).ToListAsync(cancellationToken);
        var toInsert = fromApi.Where(dto => !string.IsNullOrEmpty(dto.Id) && !existingIds.Contains(dto.Id)).Select(dto => new Launch
        {
            Id = dto.Id!,
            Name = dto.Name,
            FlightNumber = dto.FlightNumber,
            DateUtc = dto.DateUtc,
            FetchedAt = DateTime.UtcNow
        }).ToList();
        _context.Launches.AddRange(toInsert);
        await _context.SaveChangesAsync(cancellationToken);
        int total = await _context.Launches.CountAsync(cancellationToken);
        return new LaunchesSyncResult(Added: toInsert.Count, Total: total);
    }
}