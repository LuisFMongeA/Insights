using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Insights.SharedKernel.Extensions;

public static class SerilogExtensions
{
    public static WebApplicationBuilder AddSerilog(this WebApplicationBuilder builder)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateBootstrapLogger();

        builder.Host.UseSerilog((context, _, configuration) =>
        {
            // Si existe la sección Serilog en appsettings, la usa
            if (context.Configuration.GetSection("Serilog").Exists())
            {
                configuration.ReadFrom.Configuration(context.Configuration);
            }
            else
            {
                // Si no, aplica la configuración por defecto
                ApplyDefaultConfiguration(configuration, context.HostingEnvironment.ApplicationName);
            }
        });

        return builder;
    }

    private static void ApplyDefaultConfiguration(LoggerConfiguration configuration, string appName)
    {
        configuration
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore", Serilog.Events.LogEventLevel.Warning)
            .MinimumLevel.Override("System", Serilog.Events.LogEventLevel.Warning)
            .Enrich.WithProperty("Application", appName)
            .WriteTo.Console(outputTemplate:
                "[{Timestamp:HH:mm:ss} {Level:u3}] {SourceContext} - {Message:lj}{NewLine}{Exception}")
            .WriteTo.File(
                path: "logs/log-.txt",
                rollingInterval: RollingInterval.Day,
                outputTemplate:
                "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] {SourceContext} - {Message:lj}{NewLine}{Exception}");
    }
}