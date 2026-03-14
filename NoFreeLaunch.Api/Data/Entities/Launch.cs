namespace NoFreeLaunch.Api.Data.Entities;

public class Launch
{
    public string Id { get; set; } = null!;
    public int? FlightNumber { get; set; }
    public string? Name { get; set; }
    public DateTime? DateUtc { get; set; }
    public DateTime FetchedAt { get; set; }

    public ICollection<Favorite> Favorites { get; set; } = [];
}