using Insights.Domain.Models;
using Insights.Domain.Repositories;
using Insights.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Insights.Infrastructure.Data.Repositories;

public class AuditRepository(InsightsDbContext context) : IAuditRepository
{
    public async Task AddAsync(AuditEntry entry)
        => await context.AuditEntries.AddAsync(entry);

    public async Task<IEnumerable<AuditEntry>> GetAllAsync()
        => await context.AuditEntries
            .OrderByDescending(e => e.RequestedAt)
            .ToListAsync();

    public async Task<AuditEntry?> GetByIdAsync(Guid id)
        => await context.AuditEntries.FindAsync(id);

    public async Task<IEnumerable<AuditEntry>> GetByDateRangeAsync(DateTime from, DateTime to)
        => await context.AuditEntries
            .Where(e => e.RequestedAt >= from && e.RequestedAt <= to)
            .OrderByDescending(e => e.RequestedAt)
            .ToListAsync();
}