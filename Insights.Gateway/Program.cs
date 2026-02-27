using Insights.Gateway.Extensions;
using Insights.MessageBus;
using Insights.SharedKernel.Extensions;
using Serilog;


try 
{
    Log.Information("Starting Insights.Gateway");
    var builder = WebApplication.CreateBuilder(args);
    builder.AddSerilog();
    builder.Services.AddControllers();
    builder.Services.AddHttpClients(builder.Configuration);
    builder.Services.AddJwtAuthentication(builder.Configuration);
    builder.Services.AddSingleton<IMessageBus, NatsMessageBus>();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

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


