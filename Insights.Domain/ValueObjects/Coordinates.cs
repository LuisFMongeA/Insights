using Insights.Domain.Exceptions;

namespace Insights.Domain.ValueObjects;

public record Coordinates
{
    public double Lat { get; init; }
    public double Lon { get; init; }
    private Coordinates() { }

    public static Coordinates Create(double lat, double lon) {
        if (lat < -90 || lat > 90)
            throw new DomainException("Wrong latitude value");
        if (lon < -180 || lon > 180)
            throw new DomainException("Wrong longitude value");
        return new Coordinates
        {
            Lat = lat,
            Lon = lon
        };
    }
}
