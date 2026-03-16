namespace Insights.Contracts.RabbitMq;

// Routing keys que se usan al publicar y al suscribirse
// Centralizado aquí para evitar strings duplicados
public static class RabbitMqRoutingKeys
{
    public const string GeoRequested = "geo.requested";
}