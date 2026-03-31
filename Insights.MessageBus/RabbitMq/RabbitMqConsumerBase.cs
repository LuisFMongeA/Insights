using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Insights.MessageBus.RabbitMq;
public abstract class RabbitMqConsumerBase<TMessage, THandler> : BackgroundService
    where THandler : IMessageHandler<TMessage>
{
    private IConnection _connection;
    private IModel _channel;
    private readonly RabbitMqConfiguration _config;
    private readonly ILogger<RabbitMqConsumerBase<TMessage, THandler>> _logger;
    private readonly IMessageHandler<TMessage> _handler;
    protected abstract string QueueName { get; }
    protected abstract string RoutingKey { get; }
    protected virtual ushort PrefetchCount => 1; // virtual con default

    public RabbitMqConsumerBase(IOptions<RabbitMqConfiguration> config,
    ILogger<RabbitMqConsumerBase<TMessage, THandler>> logger,
    IMessageHandler<TMessage> handler)
    {
        _config = config.Value;
        _logger = logger;
        _handler = handler;


    }

    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested) 
        {
            try
            {
                Connect();
                await WaitUntilCancelledAsync(ct);

            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "RabbitMQ connection lost. Retrying in 5 seconds...");
                await Task.Delay(5000, ct);
            }
        }
    }

    private void Connect()
    {
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
        // Se define el exchange principal y sus propiedades
        _channel.ExchangeDeclare(
            exchange: _config.ExchangeName,
            type: _config.ExchangeType,
            durable: true,
            autoDelete: false);

        //  Se define el exchange de fallidos DLX(DeadLettereXchange)
        var dlxName = $"{_config.ExchangeName}.dlx";
        _channel.ExchangeDeclare(
            exchange: dlxName,
            type: "direct", // direct porque solo recibe mensajes de una cola
            durable: true,
            autoDelete: false);
        
        // Se define la cola de fallidos DLQ(DeadLetterQueue)
        var dlqName = $"{QueueName}.dlq";
        _channel.QueueDeclare(
            queue: dlqName,
            durable: true,
            exclusive: false,
            autoDelete: false);

        // Se crea la asociacion entre la DLX y la DLQ
        _channel.QueueBind(
            queue: dlqName,
            exchange: dlxName,
            routingKey: QueueName); // routing key = nombre de la cola original

        
        //Especifica cual es el nombre del DLX
        var queueArgs = new Dictionary<string, object> 
        { 
            { "x-dead-letter-exchange", dlxName },
            { "x-message-ttl", 86400000 }  // 24 horas en ms
        };

        // Se declara la cola principal
        _channel.QueueDeclare(
            queue: QueueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: queueArgs);

        // Se asocia la cola principal con el exchange
        _channel.QueueBind(
            queue: QueueName,
            exchange: _config.ExchangeName,
            routingKey: RoutingKey);
        _channel.BasicQos(prefetchSize: 0, prefetchCount: PrefetchCount, false);

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.Received += OnMessageReceivedAsync;

        _channel.BasicConsume(
            queue: QueueName,
            autoAck: false,
            consumer: consumer);

    }

    private async Task OnMessageReceivedAsync(object sender, BasicDeliverEventArgs @event)
    {
        var deliveryTag = @event.DeliveryTag;
        var body = Encoding.UTF8.GetString(@event.Body.ToArray());
        var message = JsonSerializer.Deserialize<TMessage>(body,
             new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (message is null) 
        {
            _logger.LogWarning("Received null message on queue {Queue}", QueueName);
            _channel!.BasicNack(deliveryTag, multiple: false, requeue: false);
            return;
        }
        try 
        {
            await _handler.HandleAsync(message, CancellationToken.None);
            _channel!.BasicAck(deliveryTag, multiple: false);
            _logger.LogInformation("Message processed on queue {Queue}", QueueName);
        }
        catch (Exception ex) 
        {
            _logger.LogError("Error handling message {Ex}", ex);
            _channel!.BasicNack(deliveryTag, multiple: false, requeue: false);
        }
    }
    
    private static Task WaitUntilCancelledAsync(CancellationToken ct)
    {
        var tcs = new TaskCompletionSource();
        ct.Register(() => tcs.SetResult());
        return tcs.Task;
    }
    public override void Dispose()
    {
        if(_connection?.IsOpen == true) _connection?.Close();
        if (_channel?.IsOpen == true) _channel?.Close();
        base.Dispose();
    }
}