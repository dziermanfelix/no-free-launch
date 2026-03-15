namespace NoFreeLaunch.Api.Data.Entities;

public class Favorite
{
    public int Id { get; set; }
    public string LaunchId { get; set; } = null!;
    public int UserId { get; set; }
    public DateTime CreatedAt { get; set; }

    internal Launch Launch { get; set; } = null!;
    internal User User { get; set; } = null!;
}