namespace Insights.MessageBus.RabbitMq;

public class RabbitMqConfiguration
{
    public string Host { get; set; } = "localhost";
    public int Port { get; set; } = 5672;
    public string Username { get; set; } = "guest";
    public string Password { get; set; } = "guest";
    public string ExchangeName { get; set; } = "geo-exchange";
    public string ExchangeType { get; set; } = "topic";
}