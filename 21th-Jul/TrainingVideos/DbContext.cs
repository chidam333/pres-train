namespace TrainingVideos.Context;

using TrainingVideos.Models;
using Microsoft.EntityFrameworkCore;

public class TrainingDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public TrainingDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Video> Videos { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Video>().HasKey(v => v.Id);
    }

}