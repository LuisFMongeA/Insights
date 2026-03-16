using Insights.Domain.Repositories;
using Insights.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Insights.Infrastructure.Data.Repositories;

public class UnitOfWork(
    InsightsDbContext context,
    IAuditRepository auditRepository,
    IStatRepository statRepository) : IUnitOfWork
{
    public IAuditRepository AuditRepository => auditRepository;
    public IStatRepository StatRepository => statRepository;

    public async Task<int> CommitAsync(CancellationToken ct = default)
        => await context.SaveChangesAsync(ct);

    public void RollbackAsync()
    {
        foreach (var entry in context.ChangeTracker.Entries())
        {
            entry.State = EntityState.Detached;
        }
    }


    public void Dispose()
        => context.Dispose();
}