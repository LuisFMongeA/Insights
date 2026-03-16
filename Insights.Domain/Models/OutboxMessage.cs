namespace Insights.Domain.Models;

public class OutboxMessage
{
    public Guid Id { get; set; }
    public string Type { get; set; } = string.Empty;      
    public string Payload { get; set; } = string.Empty;   // JSON del evento
    public DateTime CreatedAt { get; set; }
    public DateTime? ProcessedAt { get; set; }            // null = pendiente
    public int RetryCount { get; set; }                   // intentos fallidos
    public string? Error { get; set; }                    // último error
}

