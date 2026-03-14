namespace NoFreeLaunch.Api.Data.Entities;

public class Favorite
{
    public int Id { get; set; }
    public string LaunchId { get; set; } = null!;
    public string UserId { get; set; } = null!;
    public DateTime CreatedAt { get; set; }

    internal Launch Launch { get; set; } = null!;
}