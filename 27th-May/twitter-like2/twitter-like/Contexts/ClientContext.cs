using System.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using twitter_like.Models;

public class ClientContext : DbContext
{
    private readonly IConfiguration _configuration;

    public ClientContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = _configuration.GetConnectionString("DefaultConnection");
        optionsBuilder.UseNpgsql(connectionString);
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
    }
    public DbSet<Follow> Follow { get; set; }
    public DbSet<Comment> Comment { get; set; }
    public DbSet<Tweet> Tweet { get; set; }
    public DbSet<User> User { get; set; }
    public DbSet<Like> Like { get; set; }
    public DbSet<Retweet> Retweet { get; set; }

}