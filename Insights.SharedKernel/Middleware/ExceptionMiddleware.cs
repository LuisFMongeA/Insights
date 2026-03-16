using Insights.SharedKernel.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace Insights.SharedKernel.Middleware;

public class ExceptionMiddleware(ILogger<ExceptionMiddleware> logger):IMiddleware
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    private static async Task WriteErrorResponse(HttpContext context, HttpStatusCode statusCode, string message)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var response = new ErrorResponse
        {
            StatusCode = (int)statusCode,
            Message = message
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response, JsonOptions));
    }

    async Task IMiddleware.InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, "Error calling external service: {Message}", ex.Message);
            await WriteErrorResponse(context, HttpStatusCode.BadGateway,
                "Error communicating with an external service");
        }
        catch (OperationCanceledException ex)
        {
            logger.LogWarning(ex, "Request cancelled");
            await WriteErrorResponse(context, HttpStatusCode.RequestTimeout,
                "Request was cancelled or timed out");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception");
            await WriteErrorResponse(context, HttpStatusCode.InternalServerError,
                "An unexpected error occurred");
        }
    }
}