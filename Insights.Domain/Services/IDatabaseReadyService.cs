namespace Insights.Domain.Services;

public interface IDatabaseReadyService
{
    bool IsReady {  get; }
    void SetReady();
}
