using Insights.CitiesAPI.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient("ApiNinjas", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiNinjas:BaseUrl"]!);
    client.DefaultRequestHeaders.Add("X-Api-Key", builder.Configuration["ApiNinjas:ApiKey"]);
});

builder.Services.AddScoped<ICitiesData, CitiesData>();
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