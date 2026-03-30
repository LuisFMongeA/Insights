using Insights.Domain.Repositories;
using Insights.Domain.Services;
using Insights.Gateway.Services;
using Insights.Infrastructure.Data.Context;
using Insights.MessageBus.RabbitMq;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Moq;

namespace Insights.Tests.Integration.Infraestrcture;

public class GatewayWebApplicationFactory: WebApplicationFactory<Program>
{
    public Mock<IGeoService> MockGeoService { get; } = new();

    protected override void ConfigureWebHost(IWebHostBuilder builder) 
    {
        builder.UseEnvironment("Testing");
        builder.ConfigureServices(services =>
        {
            // Elimina servicios reales
            services.RemoveAll<IGeoService>();
            services.RemoveAll<IOutboxRepository>();
            services.RemoveAll<IRabbitMqPublisher>();
            services.RemoveAll<IMessagePublisher>();
            services.RemoveAll<IHostedService>();
            services.RemoveAll<DbContextOptions<InsightsDbContext>>();
            services.AddDbContext<InsightsDbContext>(options =>
                options.UseInMemoryDatabase("TestDb"));

            // Añade mocks
            services.AddScoped(_ => MockGeoService.Object);
            services.AddScoped(_ => new Mock<IOutboxRepository>().Object);
            services.AddSingleton(_ => new Mock<IRabbitMqPublisher>().Object);
            services.AddSingleton(_ => new Mock<IMessagePublisher>().Object);
        });

        builder.ConfigureServices(services =>
        {
            // Elimina la autenticación JWT real
            services.RemoveAll<IAuthenticationSchemeProvider>();

            // Añade autenticación fake que siempre aprueba
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "Test";
                options.DefaultChallengeScheme = "Test";
            })
            .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });
        }); 
    }
}
