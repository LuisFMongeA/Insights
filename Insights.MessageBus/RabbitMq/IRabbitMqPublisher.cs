namespace Insights.MessageBus.RabbitMq;
public interface IRabbitMqPublisher
{
    Task PublishAsync<T>(string routingKey, T message, CancellationToken ct = default);
    Task PublishAsync(string routingKey, string message, CancellationToken ct = default);
}