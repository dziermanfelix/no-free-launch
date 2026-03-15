namespace NoFreeLaunch.Api.Services;

using NoFreeLaunch.Api.Data.Entities;
using NoFreeLaunch.Api.Data;
using Microsoft.EntityFrameworkCore;

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
}