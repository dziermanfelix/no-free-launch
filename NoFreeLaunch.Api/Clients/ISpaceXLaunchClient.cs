

namespace NoFreeLaunch.Api.Clients;

public interface ISpaceXLaunchClient
{
    Task<IReadOnlyList<SpaceXLaunchDto>> GetLaunchesAsync(CancellationToken cancellationToken = default);

    Task<SpaceXLaunchDto?> GetLaunchByIdAsync(string id, CancellationToken cancellationToken = default);

    Task<SpaceXLaunchDto?> GetLaunchByFlightNumberAsync(int flightNumber, CancellationToken cancellationToken = default);
}