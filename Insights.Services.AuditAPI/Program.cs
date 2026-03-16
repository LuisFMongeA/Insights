using Insights.Infrastructure.Data.Extensions;
using Insights.SharedKernel.Extensions;
using Serilog;

try
{
    Log.Information("Starting Insights.AuditAPI");

    var builder = WebApplication.CreateBuilder(args);
    builder.AddSharedConfiguration();
    builder.AddSerilog();
    builder.Services.AddDatabase(builder.Configuration);
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddSharedMiddlewares();

    var app = builder.Build();

    app.UseExceptionMiddleware();
    app.UseApiKeyMiddleware();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Insights.AuditAPI terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}