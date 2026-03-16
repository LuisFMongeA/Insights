using Insights.Domain.Services;

namespace Insights.Infrastructure.Data.Services;

public class DatabaseReadyService : IDatabaseReadyService
{
    private bool _isReady = false;
    public bool IsReady => _isReady;
    

    public void SetReady() => _isReady = true;
}
