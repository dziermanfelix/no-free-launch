using Microsoft.EntityFrameworkCore;
using NoFreeLaunch.Api.Data.Entities;

namespace NoFreeLaunch.Api.Data;

public class NoFreeLaunchDbContext : DbContext
{
    public NoFreeLaunchDbContext(DbContextOptions<NoFreeLaunchDbContext> options)
        : base(options) { }

    public DbSet<Launch> Launches => Set<Launch>();
    public DbSet<Favorite> Favorites => Set<Favorite>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Launch>(e =>
        {
            e.ToTable("Launches");
            e.HasKey(x => x.Id);
        });

        modelBuilder.Entity<User>(e =>
        {
            e.ToTable("Users");
            e.HasKey(x => x.Id);
        });

        modelBuilder.Entity<Favorite>(e =>
        {
            e.ToTable("Favorites");
            e.HasKey(x => x.Id);
            e.HasIndex(x => new { x.LaunchId, x.UserId }).IsUnique();
            e.HasOne(x => x.Launch)
             .WithMany(x => x.Favorites)
             .HasForeignKey(x => x.LaunchId)
             .OnDelete(DeleteBehavior.Cascade);
            e.HasOne(x => x.User)
             .WithMany(x => x.Favorites)
             .HasForeignKey(x => x.UserId)
             .OnDelete(DeleteBehavior.Cascade);
        });
    }
}