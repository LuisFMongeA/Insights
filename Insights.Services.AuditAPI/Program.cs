


using Insights.AuditAPI.Extensions;
using Insights.SharedKernel.Extensions;
using Serilog;

try 
{
    Log.Information("Starting Insights.AuditAPI");
    var builder = WebApplication.CreateBuilder(args);
    builder.AddSerilog();

    // Add services to the container.

    builder.Services.AddControllers();
    builder.Services.AddNatsConsumer();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();
    app.UseExceptionMiddleware();

    // Configure the HTTP request pipeline.
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
    Log.Fatal(ex, "Insights.AuditApi terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

