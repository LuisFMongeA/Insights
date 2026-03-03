using Insights.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Insights.Infrastructure.Data.Context;

public class InsightsDbContext(DbContextOptions<InsightsDbContext> options)
    : DbContext(options)
{
    public DbSet<AuditEntry> AuditEntries => Set<AuditEntry>();
    public DbSet<StatEntry> StatEntries => Set<StatEntry>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AuditEntry>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.RequestedAt);
            entity.HasIndex(e => e.ResolvedCityName);
        });

        modelBuilder.Entity<StatEntry>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.CityName);
            entity.HasIndex(e => e.CountryCode);
            entity.HasIndex(e => e.RequestedAt);
        });
    }
}