
using Insights.Contracts.Events;
using Insights.Contracts.RabbitMq;
using Insights.MessageBus;
using Insights.MessageBus.RabbitMq;
using Insights.Processor.Handlers;
using Microsoft.Extensions.Options;

namespace Insights.Processor.Consumers;

public class GeoStatsConsumer : RabbitMqConsumerBase<GeoInfoRequestedEvent, IGeoAuditHandler>
{
    public GeoStatsConsumer(IOptions<RabbitMqConfiguration> config, ILogger<RabbitMqConsumerBase<GeoInfoRequestedEvent, IGeoAuditHandler>> logger, IGeoStatsHandler handler) : base(config, logger, handler)
    {
    }

    protected override string QueueName => RabbitMqQueueNames.Stats;

    protected override string RoutingKey => RabbitMqRoutingKeys.GeoRequested;
}
