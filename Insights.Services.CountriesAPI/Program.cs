using Insights.CountriesAPI.Data;
using Insights.SharedKernel.Extensions;
using Serilog;

try
{
    Log.Information("Starting Insights.CountriesAPI");
    var builder = WebApplication.CreateBuilder(args);
    builder.AddSharedConfiguration();
    builder.AddSerilog();

    builder.Services.AddHttpClient("RestCountries", client =>
    {
        client.BaseAddress = new Uri(builder.Configuration["RestCountries:BaseUrl"]!);
    });

    builder.Services.AddScoped<ICountriesData, CountriesData>();
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

    Log.Fatal(ex, "Insights.CountriesAPI terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

