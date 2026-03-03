using Insights.MessageBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NATS.Client.Core;
using NATS.Client.Serializers.Json;

public class NatsMessageBus : IMessageBus, IAsyncDisposable
{
    private readonly NatsConnection _connection;
    private readonly ILogger<NatsMessageBus> _logger;

    public NatsMessageBus(IConfiguration configuration, ILogger<NatsMessageBus> logger)
    {
        _logger = logger;
        var url = configuration["Nats:Url"] ?? "nats://localhost:4222";
        _connection = new NatsConnection(new NatsOpts
        {
            Url = url,
            SerializerRegistry = NatsJsonSerializerRegistry.Default
        });
    }

    public async Task PublishAsync<T>(string subject, T message, CancellationToken ct = default)
    {
        try
        {
            _logger.LogInformation("Publishing to NATS subject: {Subject}", subject);
            await _connection.PublishAsync(subject, message, cancellationToken: ct);
            _logger.LogInformation("Published successfully to: {Subject}", subject);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error publishing to NATS subject: {Subject}", subject);
            throw;
        }
    }

    public async ValueTask DisposeAsync()
    {
        await _connection.DisposeAsync();
    }
}