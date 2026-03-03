using Insights.SharedKernel.Extensions;
using Insights.WeatherAPI.Data;
using Serilog;

try
{
    Log.Information("Starting Insights.WeatherAPI");
    var builder = WebApplication.CreateBuilder(args);
    builder.AddSerilog();

    var apiKey = builder.Configuration["OpenWeather:ApiKey"];

    builder.Services.AddHttpClient("OpenWeather", client =>
    {
        var baseUrl = builder.Configuration["OpenWeather:BaseUrl"]!;
        // OpenWeather usa appid como query param, lo añadimos como BaseAddress con el param fijo
        client.BaseAddress = new Uri(baseUrl);
        client.DefaultRequestHeaders.Add("Accept", "application/json");
    });

    // Registramos el apiKey en los endpoints via configuración, 
    // WeatherData lo añade al endpoint en cada llamada
    builder.Services.AddScoped<IWeatherData, WeatherData>();
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

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
    Log.Fatal(ex, "Insights.WeatherAPI terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

