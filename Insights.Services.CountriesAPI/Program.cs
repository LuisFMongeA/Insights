using Insights.CountriesAPI.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient("RestCountries", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["RestCountries:BaseUrl"]!);
});

builder.Services.AddScoped<ICountriesData, CountriesData>();
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