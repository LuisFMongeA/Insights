using Insights.Domain.Exceptions;

namespace Insights.Domain.ValueObjects;

public record CountryCode 
{
    public string Code { get; init; }
    private CountryCode() { }
    public static CountryCode Create(string code) 
    {
        ArgumentNullException.ThrowIfNull(code, nameof(code));
        if (code.Length !=2) throw new DomainException("Country Code length invalid");
        return new CountryCode() { Code = code.ToUpperInvariant() };
    }
}
