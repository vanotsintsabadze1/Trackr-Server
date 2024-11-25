using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Trackr.Domain.Models;

namespace Trackr.Infrastructure.Context;

public class TrackrDBContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    private readonly IConfiguration _configuration;

    public TrackrDBContext(IConfiguration configuration, DbContextOptions<TrackrDBContext> options) : base(options)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = _configuration.GetConnectionString("DefaultConnection");
        optionsBuilder.UseSqlServer(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TrackrDBContext).Assembly);
    }
}

public class TrackrDBContextFactory : IDesignTimeDbContextFactory<TrackrDBContext>
{
    public TrackrDBContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder().AddUserSecrets<TrackrDBContext>().Build();
        var optionsBuilder = new DbContextOptionsBuilder<TrackrDBContext>();
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        optionsBuilder.UseSqlServer(connectionString);
        return new TrackrDBContext(configuration, optionsBuilder.Options);
    }
}