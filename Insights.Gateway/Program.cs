using Insights.Domain.Services;
using Insights.Gateway.Extensions;
using Insights.Gateway.Services;
using Insights.Infrastructure.Data.Extensions;
using Insights.MessageBus;
using Insights.MessageBus.Extensions;
using Insights.SharedKernel.Extensions;

using Serilog;


try 
{
    Log.Information("Starting Insights.Gateway");
    var builder = WebApplication.CreateBuilder(args);
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

    var app = builder.Build();
    app.UseExceptionMiddleware();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.UseScopeMiddleware();
    app.MapControllers();
    app.Run();

} catch (Exception ex)
{
    Log.Fatal(ex, "Insights.Gateway terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}


