namespace Insights.Domain.Services;

public interface IMessagePublisher
{
    Task PublishAsync(string type, string payload);
}
