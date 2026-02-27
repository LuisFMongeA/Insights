using Insights.AuthAPI.Extensions;
using Insights.SharedKernel.Extensions;
using Serilog;

try
{
    Log.Information("Starting Insights.AuthAPI");

    var builder = WebApplication.CreateBuilder(args);

    builder.AddSerilog();
    builder.Services.AddAuthServices();
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new() { Title = "Insights.AuthAPI", Version = "v1" });

        // Añade soporte para Basic Auth en Swagger
        c.AddSecurityDefinition("Basic", new()
        {
            Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
            Scheme = "basic",
            Description = "Introduce clientId y clientSecret"
        });

        c.AddSecurityRequirement(new()
    {
        {
            new()
            {
                Reference = new()
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Basic"
                }
            },
            []
        }
    });
    });

    var app = builder.Build();

    app.UseExceptionMiddleware();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.UseRouting();
    app.MapControllers();
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Insights.AuthAPI terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}