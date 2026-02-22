using Microsoft.Extensions.Configuration;
using NATS.Client.Core;

namespace Insights.MessageBus;

public class NatsMessageBus : IMessageBus, IAsyncDisposable
{
    private readonly NatsConnection _connection;

    public NatsMessageBus(IConfiguration configuration)
    {
        var url = configuration["Nats:Url"] ?? "nats://localhost:4222";
        _connection = new NatsConnection(new NatsOpts { Url = url });
    }

    public async Task PublishAsync<T>(string subject, T message, CancellationToken ct = default)
    {
        await _connection.PublishAsync(subject, message, cancellationToken: ct);
    }

    public async ValueTask DisposeAsync()
    {
        await _connection.DisposeAsync();
    }
}