namespace Insights.Contracts.RabbitMq;

// Nombres de las colas
// Centralizados para que publisher y consumer usen exactamente el mismo nombre
public static class RabbitMqQueueNames
{
    public const string Audit = "audit-queue";
    public const string Stats = "stats-queue";
}