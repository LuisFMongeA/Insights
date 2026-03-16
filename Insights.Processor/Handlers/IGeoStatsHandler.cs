using Insights.Contracts.Events;
using Insights.MessageBus;

namespace Insights.Processor.Handlers;

public interface IGeoStatsHandler : IMessageHandler<GeoInfoRequestedEvent> { }
