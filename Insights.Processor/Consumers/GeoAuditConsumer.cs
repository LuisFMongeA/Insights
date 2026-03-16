
using Insights.Contracts.Events;
using Insights.Contracts.RabbitMq;
using Insights.MessageBus;
using Insights.MessageBus.RabbitMq;
using Insights.Processor.Handlers;
using Microsoft.Extensions.Options;

namespace Insights.Processor.Consumers;

public class GeoAuditConsumer : RabbitMqConsumerBase<GeoInfoRequestedEvent, IGeoAuditHandler>
{
    public GeoAuditConsumer(IOptions<RabbitMqConfiguration> config, ILogger<RabbitMqConsumerBase<GeoInfoRequestedEvent, IGeoAuditHandler>> logger, IGeoAuditHandler handler) : base(config, logger, handler)
    {
    }

    protected override string QueueName => RabbitMqQueueNames.Audit;

    protected override string RoutingKey => RabbitMqRoutingKeys.GeoRequested;
}
