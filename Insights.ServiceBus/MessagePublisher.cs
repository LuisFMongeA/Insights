using Insights.Domain.Services;
using Insights.MessageBus.RabbitMq;
using Microsoft.Extensions.Logging;

namespace Insights.MessageBus;

public class MessagePublisher(
  IRabbitMqPublisher rabbitMqPublisher,
  ILogger<MessagePublisher> logger) : IMessagePublisher
{
    public Task PublishAsync(string type, string payload)
    {
        logger.LogInformation("Publishing message of type: {Type}", type);
        return rabbitMqPublisher.PublishAsync(type, payload);
    }
}
