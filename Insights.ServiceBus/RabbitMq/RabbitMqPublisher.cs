using Insights.MessageBus.RabbitMq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Insights.MessageBus.RabbitMq;

public class RabbitMqPublisher : IRabbitMqPublisher, IDisposable
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly RabbitMqConfiguration _config;
    private readonly ILogger<RabbitMqPublisher> _logger;

    public RabbitMqPublisher(IOptions<RabbitMqConfiguration> config,
    ILogger<RabbitMqPublisher> logger) 
    {
        _config = config.Value;
        _logger= logger;
        var factory = new ConnectionFactory
        {
            HostName = _config.Host,
            Port = _config.Port,
            UserName = _config.Username,
            Password = _config.Password,
            DispatchConsumersAsync = true  // permite consumers async/await
        };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.ExchangeDeclare(
         exchange: _config.ExchangeName,   // "geo-exchange"
         type: _config.ExchangeType,       // "topic"
         durable: true,
         autoDelete: false,
         arguments: null);
    }

    public Task PublishAsync<T>(string routingKey, T message, CancellationToken ct = default)
    {
        try
        {
            byte[] bytes = JsonSerializer.SerializeToUtf8Bytes(message);
            IBasicProperties properties = _channel.CreateBasicProperties();
            properties.Persistent = true;
            properties.ContentType = "application/json";
            properties.Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds());

            _channel.BasicPublish(exchange: _config.ExchangeName, routingKey: routingKey, basicProperties: properties, body: bytes);
            _logger.LogInformation("Publicado mensaje a {RoutingKey}", routingKey);
        } catch (Exception ex) {

            _logger.LogError("Error publicando mensaje a {RoutingKey}: {Error}", routingKey, ex.Message);
            throw; // relanza la excepción para que el caller la gestione
        }
        return Task.CompletedTask;
    }


    public void Dispose()
    {
        if (_channel?.IsOpen == true) _channel.Close();
        if (_connection?.IsOpen == true) _connection.Close();
    }

    public Task PublishAsync(string routingKey, string message, CancellationToken ct = default)
    {
        try
        {
            byte[] bytes = Encoding.UTF8.GetBytes(message);
            IBasicProperties properties = _channel.CreateBasicProperties();
            properties.Persistent = true;
            properties.ContentType = "application/json";
            properties.Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds());

            _channel.BasicPublish(exchange: _config.ExchangeName, routingKey: routingKey, basicProperties: properties, body: bytes);
            _logger.LogInformation("Publicado mensaje a {RoutingKey}", routingKey);
        }
        catch (Exception ex)
        {

            _logger.LogError("Error publicando mensaje a {RoutingKey}: {Error}", routingKey, ex.Message);
            throw; // relanza la excepción para que el caller la gestione
        }
        return Task.CompletedTask;
    }
}