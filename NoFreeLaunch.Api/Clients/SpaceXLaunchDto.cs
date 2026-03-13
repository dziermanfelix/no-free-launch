using System.Text.Json.Serialization;

namespace NoFreeLaunch.Api.Clients;

public sealed class SpaceXLaunchDto
{
    public string? Id { get; set; }
    [JsonPropertyName("flight_number")]
    public int? FlightNumber { get; set; }
    public string? Name { get; set; }
    [JsonPropertyName("date_utc")]
    public DateTime? DateUtc { get; set; }
}