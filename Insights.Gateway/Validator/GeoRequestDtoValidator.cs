using FluentValidation;
using Insights.Gateway.Model;
using System.Net;
using System.Text.RegularExpressions;

namespace Insights.Gateway.Validator;

public class GeoRequestDtoValidator : AbstractValidator<GeoRequestDto>
{
    public GeoRequestDtoValidator() 
    {
        RuleFor(x => x.Lat)
            .InclusiveBetween(-90, 90)
            .WithMessage("Lat value incorrect")
            .When(x => x.Lat.HasValue);

        RuleFor(x => x.Lon)
            .InclusiveBetween(-180, 180)
            .WithMessage("Lon value incorrect")
            .When(x => x.Lon.HasValue);

        RuleFor(x => x)
            .Must(x => x.Lat.HasValue == x.Lon.HasValue)
            .WithMessage("Lat and Lon must both be provided or both be empty");

        RuleFor(x => x.CityName)
            .MaximumLength(100)
            .Matches(@"^[a-zA-Z\s\-]+$")
            .WithMessage("City name with invalid characters")
            .When(x => !string.IsNullOrEmpty(x.CityName));

        RuleFor(x => x.IpAddress)
            .Must(ip =>
            {
                // IPv4 — exactamente X.X.X.X
                var ipv4Regex = @"^(\d{1,3}\.){3}\d{1,3}$";
                // IPv6 — contiene :
                var ipv6Regex = @"^[0-9a-fA-F:]+$";

                if (Regex.IsMatch(ip, ipv4Regex))
                    return IPAddress.TryParse(ip, out _);

                if (ip.Contains(':') && Regex.IsMatch(ip, ipv6Regex))
                    return IPAddress.TryParse(ip, out _);

                return false;
            })
            .WithMessage("Invalid IP address format — must be IPv4 or IPv6")
            .When(x => !string.IsNullOrEmpty(x.IpAddress));

        RuleFor(x => x)
            .Must(x =>
                !string.IsNullOrEmpty(x.CityName) ||
                (x.Lat.HasValue && x.Lon.HasValue) ||
                !string.IsNullOrEmpty(x.IpAddress))
            .WithMessage("Provide CityName, Lat&Lon, or IpAddress");
    }
}
