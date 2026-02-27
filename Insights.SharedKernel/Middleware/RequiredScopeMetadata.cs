namespace Insights.SharedKernel.Middleware;

public class RequiredScopeMetadata(string scope)
{
    public string Scope { get; } = scope;
}