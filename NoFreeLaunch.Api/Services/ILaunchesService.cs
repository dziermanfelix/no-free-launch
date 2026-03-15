namespace NoFreeLaunch.Api.Services;

using NoFreeLaunch.Api.Clients;
using NoFreeLaunch.Api.Data.Entities;

public record LaunchesSyncResult(int Added, int Total);

public interface ILaunchesService
{
    Task<IReadOnlyList<Launch>> GetLaunchesAsync(CancellationToken cancellationToken = default);
    Task<Launch?> GetLaunchAsync(string id, CancellationToken cancellationToken = default);
    Task<Launch?> GetLaunchByFlightNumberAsync(int flightNumber, CancellationToken cancellationToken = default);
    Task<LaunchesSyncResult> LaunchesSyncAsync(ISpaceXLaunchClient client, CancellationToken cancellationToken = default);
}