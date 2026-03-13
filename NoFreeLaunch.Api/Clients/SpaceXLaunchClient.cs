namespace NoFreeLaunch.Api.Clients;

public sealed class SpaceXLaunchClient : ISpaceXLaunchClient
{
    private readonly HttpClient _httpClient;

    public SpaceXLaunchClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IReadOnlyList<SpaceXLaunchDto>> GetLaunchesAsync(CancellationToken cancellationToken = default)
    {
        var launches = await _httpClient.GetFromJsonAsync<IReadOnlyList<SpaceXLaunchDto>>("v5/launches", cancellationToken);
        return launches ?? [];
    }

    public async Task<SpaceXLaunchDto?> GetLaunchByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return await _httpClient.GetFromJsonAsync<SpaceXLaunchDto>($"v5/launches/{id}", cancellationToken);
    }

}