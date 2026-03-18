using Insights.AuthAPI.Extensions;
using Insights.SharedKernel.Extensions;
using Serilog;
using System.Threading.RateLimiting;

try
{
    Log.Information("Starting Insights.AuthAPI");

    var builder = WebApplication.CreateBuilder(args);
    builder.WebHost.ConfigureKestrel(options =>
    {
        options.AddServerHeader = false;
    });
    builder.AddSharedConfiguration();
    builder.AddSerilog();
    builder.Services.AddAuthServices();
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new() { Title = "Insights.AuthAPI", Version = "v1" });

        // Añade soporte para Basic Auth en Swagger
        c.AddSecurityDefinition("Basic", new()
        {
            Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
            Scheme = "basic",
            Description = "Introduce clientId y clientSecret"
        });

        c.AddSecurityRequirement(new()
    {
        {
            new()
            {
                Reference = new()
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Basic"
                }
            },
            []
        }
    });
    });
    builder.Services.AddRateLimiter(options =>
    {
        options.RejectionStatusCode = 429;
        options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
            RateLimitPartition.GetFixedWindowLimiter(
                partitionKey: context.Connection?.RemoteIpAddress?.ToString() ?? "anonymous",
                factory: _ => new FixedWindowRateLimiterOptions
                {
                    PermitLimit = 10,
                    Window = TimeSpan.FromMinutes(1),
                    QueueLimit = 0
                }));
    });

    builder.Services.AddSharedMiddlewares();

    var app = builder.Build();
    app.UseExceptionMiddleware();
    app.UseSecurityHeadersMiddleware();
    app.UseHttpsRedirection();
    app.UseRateLimiter();
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseRouting();
    app.MapControllers();


    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }


    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Insights.AuthAPI terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}