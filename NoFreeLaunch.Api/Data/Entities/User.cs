namespace NoFreeLaunch.Api.Data.Entities;

using HotChocolate;

public class User
{
    public int Id { get; set; }
    public string UserName { get; set; } = null!;

    [GraphQLIgnore]
    public string? PasswordHash { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Favorite> Favorites { get; set; } = [];
}