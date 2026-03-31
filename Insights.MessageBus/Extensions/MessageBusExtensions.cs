using Insights.Domain.Services;
using Insights.MessageBus.RabbitMq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Insights.MessageBus.Extensions;

public static class MessageBusExtensions
{
    public static IServiceCollection AddRabbitMqPublisher(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Lee la configuración de RabbitMQ desde appsettings
        // y la registra como IOptions<RabbitMqConfiguration>
        ConfigureRabbitMq(services, configuration);

        // Singleton porque la conexión TCP se crea una sola vez
        services.AddSingleton<IRabbitMqPublisher, RabbitMqPublisher>();
        services.AddSingleton<IMessagePublisher, MessagePublisher>();

        return services;
    }

    public static IServiceCollection AddRabbitMqConsumer<TConsumer, THandler, THandlerInterface, TMessage>(
        this IServiceCollection services,
        IConfiguration configuration)
        where TConsumer : BackgroundService
        where THandler : class, THandlerInterface
        where THandlerInterface : class, IMessageHandler<TMessage>
    {
        // Configura RabbitMQ si no está ya configurado
        ConfigureRabbitMq(services, configuration);

        // Singleton porque BackgroundService vive toda la aplicación
        services.AddHostedService<TConsumer>();
        services.AddSingleton<THandlerInterface, THandler>();

        return services;
    }

    private static void ConfigureRabbitMq(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RabbitMqConfiguration>(
            configuration.GetSection("RabbitMq"));
    }
}