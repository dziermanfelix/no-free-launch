namespace NoFreeLaunch.Api.Services;

using NoFreeLaunch.Api.Data.Entities;

public interface ILaunchesService
{
    Task<IReadOnlyList<Launch>> GetLaunchesAsync(CancellationToken cancellationToken = default);
    Task<Launch?> GetLaunchAsync(string id, CancellationToken cancellationToken = default);
    Task<Launch?> GetLaunchByFlightNumberAsync(int flightNumber, CancellationToken cancellationToken = default);
}