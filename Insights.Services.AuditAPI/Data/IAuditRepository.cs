using Insights.Services.AuditAPI.Model;

namespace Insights.AuditAPI.Data;

public interface IAuditRepository
{
    Task SaveAsync(AuditEntry entry);
    Task<IEnumerable<AuditEntry>> GetAllAsync();
    Task<AuditEntry?> GetByIdAsync(Guid id);
}