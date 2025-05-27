using System.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

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
    public DbSet<Patient> Patients { get; set; }
}