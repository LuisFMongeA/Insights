using FluentValidation;
using Insights.Domain.Services;
using Insights.Gateway.Extensions;
using Insights.Gateway.Filters;
using Insights.Gateway.HttpClients;
using Insights.Gateway.Services;
using Insights.Gateway.Strategies;
using Insights.Gateway.Validator;
using Insights.Infrastructure.Data.Extensions;
using Insights.MessageBus;
using Insights.MessageBus.Extensions;
using Insights.SharedKernel.Extensions;
using Microsoft.Extensions.Caching.Memory;
using Serilog;
using System.Threading.RateLimiting;


try 
{
    Log.Information("Starting Insights.Gateway");
    var builder = WebApplication.CreateBuilder(args);
    builder.WebHost.ConfigureKestrel(options =>
    {
        options.AddServerHeader = false;
    });
    builder.AddSharedConfiguration();
    builder.AddSerilog();
    builder.Services.AddControllers();
    builder.Services.AddHttpClients(builder.Configuration);
    builder.Services.AddJwtAuthentication(builder.Configuration);
    builder.Services.AddScoped<IGeoService, GeoService>();
    builder.Services.AddSingleton<IMessagePublisher, MessagePublisher>();
    builder.Services.AddRabbitMqPublisher(builder.Configuration);
    builder.Services.AddDatabase(builder.Configuration);
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddSharedMiddlewares();
    builder.Services.AddRateLimiter(options =>
    {
        options.RejectionStatusCode = 429; // Too Many Requests

        options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
            RateLimitPartition.GetFixedWindowLimiter(
                partitionKey: context.User?.Identity?.Name ?? context.Connection?.RemoteIpAddress?.ToString() ?? "anonymous",
                factory: _ => new FixedWindowRateLimiterOptions
                {
                    PermitLimit = 100,
                    Window = TimeSpan.FromMinutes(1)
                }));
    });
    builder.Services.AddValidatorsFromAssemblyContaining<GeoRequestDtoValidator>();
    builder.Services.AddScoped(typeof(ValidationFilter<>));
    builder.Services.AddMemoryCache();
    builder.Services.AddScoped<GeoService>();
    builder.Services.AddScoped<IGeoService, CachedGeoService>(sp =>
        new CachedGeoService(
            sp.GetRequiredService<GeoService>(),
            sp.GetRequiredService<IMemoryCache>(),
            sp.GetRequiredService<ILogger<CachedGeoService>>()
    ));
    builder.Services.AddScoped<ICityResolutionStrategy, CityByNameStrategy>();
    builder.Services.AddScoped<ICityResolutionStrategy, CityByCoordinatesStrategy>();
    builder.Services.AddScoped<ICityResolutionStrategy, CityByIpStrategy>();
    builder.Services.AddScoped<IWeatherHttpClient, WeatherHttpClient>();
    builder.Services.AddScoped<ICountriesHttpClient, CountriesHttpClient>();

    var app = builder.Build();
    app.UseExceptionMiddleware();
    app.UseSecurityHeadersMiddleware();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseHttpsRedirection();
    }

    app.UseHttpsRedirection();
    app.UseRateLimiter();
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseScopeMiddleware();
    app.MapControllers();
    app.Run();

} catch (Exception ex)
{
    Log.Fatal(ex, "Insights.Gateway terminated unexpectedly");
    throw;
}
finally
{
    Log.CloseAndFlush();
}

public partial class Program { }
