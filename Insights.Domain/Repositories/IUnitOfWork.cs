namespace Insights.Domain.Repositories;

public interface IUnitOfWork : IDisposable
{
    IAuditRepository AuditRepository { get; }
    IStatRepository StatRepository { get; }
    Task<int> CommitAsync(CancellationToken ct = default);
    Task RollbackAsync();
}