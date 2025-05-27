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
    public DbSet<Follow> Follow { get; set; }
    public DbSet<Comment> Comment { get; set; }   
}