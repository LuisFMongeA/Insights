using Insights.Contracts.Events;

namespace Insights.Gateway.Services;

public interface IEventPublisher
{
    Task PublishGeoRequestedAsync(GeoInfoRequestedEvent evt);
}