using Insights.Domain.Repositories;
using Insights.Infrastructure.Data.Context;

namespace Insights.Infrastructure.Data.Repositories;

public class UnitOfWork(InsightsDbContext context) : IUnitOfWork
{
    private IAuditRepository? _auditRepository;
    private IStatRepository? _statRepository;

    // Lazy initialization: solo crea el repositorio cuando se necesita
    public IAuditRepository AuditRepository
        => _auditRepository ??= new AuditRepository(context);

    public IStatRepository StatRepository
        => _statRepository ??= new StatRepository(context);

    public async Task<int> CommitAsync(CancellationToken ct = default)
        => await context.SaveChangesAsync(ct);

    public async Task RollbackAsync()
        => await context.DisposeAsync().AsTask();

    public void Dispose()
        => context.Dispose();
}