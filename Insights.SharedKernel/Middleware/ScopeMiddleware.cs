using Insights.SharedKernel.Constants;
using Insights.SharedKernel.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace Insights.SharedKernel.Middleware;

public class ScopeMiddleware(
    RequestDelegate next,
    ILogger<ScopeMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        // Si el endpoint no requiere scope específico, deja pasar
        var endpoint = context.GetEndpoint();
        var requiredScopeMetadata = endpoint?.Metadata
            .GetMetadata<RequiredScopeMetadata>();

        if (requiredScopeMetadata is null)
        {
            await next(context);
            return;
        }

        // Obtiene los scopes del JWT
        var userScopes = context.User.Claims
            .Where(c => c.Type == AuthConstants.ScopeClaimType)
            .Select(c => c.Value)
            .ToList();

        var hasScope = userScopes.Contains(requiredScopeMetadata.Scope)
                    || userScopes.Contains(AuthConstants.Scopes.Admin);

        if (!hasScope)
        {
            logger.LogWarning(
                "Client {ClientId} attempted to access {Path} without required scope {Scope}",
                context.User.FindFirst("client_id")?.Value ?? "unknown",
                context.Request.Path,
                requiredScopeMetadata.Scope);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.Forbidden;

            var response = new ErrorResponse
            {
                StatusCode = (int)HttpStatusCode.Forbidden,
                Message = $"Required scope: {requiredScopeMetadata.Scope}"
            };

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(response,
                    new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));
            return;
        }

        await next(context);
    }
}