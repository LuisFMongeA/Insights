using Insights.Domain.Repositories;
using Insights.Domain.Services;
using Insights.Gateway.Services;
using Insights.MessageBus.RabbitMq;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;

namespace Insights.Tests.Integration.Infraestrcture;

public class GatewayWebApplicationFactory: WebApplicationFactory<Program>
{
    public Mock<IGeoService> MockGeoService { get; } = new();

    protected override void ConfigureWebHost(IWebHostBuilder builder) 
    {
        builder.ConfigureServices(services =>
        {
            // Elimina servicios reales
            services.RemoveAll<IGeoService>();
            services.RemoveAll<IOutboxRepository>();
            services.RemoveAll<IRabbitMqPublisher>();
            services.RemoveAll<IMessagePublisher>();

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
