using Insights.CitiesAPI.Data;
using Insights.SharedKernel.Extensions;
using Serilog;

try
{
    Log.Information("Starting Insights.CitiesAPI");
    var builder = WebApplication.CreateBuilder(args);
    builder.AddSerilog();

    builder.Services.AddHttpClient("ApiNinjas", client =>
    {
        client.BaseAddress = new Uri(builder.Configuration["ApiNinjas:BaseUrl"]!);
        client.DefaultRequestHeaders.Add("X-Api-Key", builder.Configuration["ApiNinjas:ApiKey"]);
    });

    builder.Services.AddHttpClient("IpApi", client =>
    {
        client.BaseAddress = new Uri(builder.Configuration["IpApi:BaseUrl"]!);
    });


    builder.Services.AddScoped<ICitiesData, CitiesData>();
    builder.Services.AddControllers();
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
    app.MapControllers();
    app.Run();

}
catch (Exception ex)
{

    Log.Fatal(ex, "Insights.CitiesAPI terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

