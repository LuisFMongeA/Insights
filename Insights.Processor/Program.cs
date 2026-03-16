using Insights.Contracts.Events;
using Insights.Infrastructure.Data.Extensions;
using Insights.MessageBus.Extensions;
using Insights.Processor.Consumers;
using Insights.Processor.Handlers;


var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddRabbitMqConsumer<GeoAuditConsumer, GeoAuditHandler, IGeoAuditHandler, GeoInfoRequestedEvent>(builder.Configuration);
builder.Services.AddRabbitMqConsumer<GeoStatsConsumer, GeoStatsHandler, IGeoStatsHandler, GeoInfoRequestedEvent>(builder.Configuration);
builder.Services.AddRabbitMqPublisher(builder.Configuration);
var host = builder.Build();


host.Run();
