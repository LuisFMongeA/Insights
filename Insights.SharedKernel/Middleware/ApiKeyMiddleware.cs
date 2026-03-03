using Insights.SharedKernel.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace Insights.SharedKernel.Middleware;

public class ApiKeyMiddleware(
    RequestDelegate next,
    IConfiguration configuration,
    ILogger<ApiKeyMiddleware> logger)
{
    private const string ApiKeyHeader = "X-Api-Key";

    public async Task InvokeAsync(HttpContext context)
    {
        // Swagger no necesita API Key en desarrollo
        if (context.Request.Path.StartsWithSegments("/swagger"))
        {
            await next(context);
            return;
        }

        if (!context.Request.Headers.TryGetValue(ApiKeyHeader, out var receivedKey))
        {
            logger.LogWarning("Request to {Path} missing API Key header", context.Request.Path);
            await WriteErrorResponse(context, HttpStatusCode.Unauthorized, "API Key required");
            return;
        }

        var validKey = configuration["ApiKey"];
        if (validKey != receivedKey)
        {
            logger.LogWarning("Invalid API Key attempt on {Path}", context.Request.Path);
            await WriteErrorResponse(context, HttpStatusCode.Forbidden, "Invalid API Key");
            return;
        }

        await next(context);
    }

    private static async Task WriteErrorResponse(
        HttpContext context,
        HttpStatusCode statusCode,
        string message)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var response = new ErrorResponse
        {
            StatusCode = (int)statusCode,
            Message = message
        };

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(response,
                new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                }));
    }
}