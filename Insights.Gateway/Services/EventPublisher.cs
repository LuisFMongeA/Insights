using Insights.Contracts.Events;
using Insights.Contracts.RabbitMq;
using Insights.MessageBus.RabbitMq;

namespace Insights.Gateway.Services;

public class EventPublisher(
    IRabbitMqPublisher rabbitMqPublisher,
    ILogger<EventPublisher> logger) : IEventPublisher
{
    public Task PublishGeoRequestedAsync(GeoInfoRequestedEvent evt)
    {
        logger.LogInformation(
            "Publishing GeoRequested event for city: {City}",
            evt.ResolvedCityName);

        return rabbitMqPublisher.PublishAsync(
            RabbitMqRoutingKeys.GeoRequested, evt);
    }
}