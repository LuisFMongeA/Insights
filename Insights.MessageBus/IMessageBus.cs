namespace Insights.MessageBus;

public interface IMessageBus
{
    Task PublishAsync<T>(string subject, T message, CancellationToken ct = default);
}