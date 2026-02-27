using Insights.SharedKernel.Middleware;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Insights.SharedKernel.Attributes;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class RequireScopeAttribute(string scope) : Attribute, IFilterMetadata
{
    public RequiredScopeMetadata Metadata { get; } = new(scope);
}