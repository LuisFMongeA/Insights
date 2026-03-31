namespace Insights.MessageBus;

public interface IMessageHandler<TMessage>
{
    Task HandleAsync(TMessage message, CancellationToken ct=default);
}
