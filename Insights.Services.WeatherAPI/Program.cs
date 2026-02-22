using Insights.WeatherAPI.Data;

var builder = WebApplication.CreateBuilder(args);

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

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();