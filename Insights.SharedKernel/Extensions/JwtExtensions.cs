using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Text.Json;

namespace Insights.SharedKernel.Extensions;

public static class JwtExtensions
{
    public static IServiceCollection AddJwtAuthentication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var authApiUrl = configuration["ApiInfo:Auth:AuthApiUrl"]
            ?? throw new InvalidOperationException("Auth:AuthApiUrl not configured");

        var issuer = configuration["Auth:Issuer"] ?? "insights-auth";
        var audience = configuration["Auth:Audience"] ?? "insights-gateway";
        var jwksUrl = $"{authApiUrl}/.well-known/jwks.json";

        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKeyResolver = (token, securityToken, kid, parameters) =>
                        GetSigningKeysFromJwks(jwksUrl),
                    ClockSkew = TimeSpan.Zero
                };

                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception is SecurityTokenExpiredException)
                            context.Response.Headers["Token-Expired"] = "true";
                        return Task.CompletedTask;
                    }
                };
            });

        services.AddAuthorization();
        return services;
    }

    // Caché estática con tiempo de expiración
    private static IEnumerable<SecurityKey>? _cachedKeys;
    private static DateTime _cacheExpiry = DateTime.MinValue;
    private static readonly TimeSpan CacheDuration = TimeSpan.FromHours(1);

    private static IEnumerable<SecurityKey> GetSigningKeysFromJwks(string jwksUrl)
    {
        if (_cachedKeys != null && DateTime.UtcNow < _cacheExpiry)
            return _cachedKeys;

        using var httpClient = new HttpClient();
        var json = httpClient.GetStringAsync(jwksUrl).GetAwaiter().GetResult();
        var jwks = JsonSerializer.Deserialize<JsonElement>(json);

        var keys = new List<SecurityKey>();

        foreach (var key in jwks.GetProperty("keys").EnumerateArray())
        {
            var kty = key.GetProperty("kty").GetString();
            if (kty != "RSA") continue;

            var n = Base64UrlDecode(key.GetProperty("n").GetString()!);
            var e = Base64UrlDecode(key.GetProperty("e").GetString()!);

            var rsa = RSA.Create();
            rsa.ImportParameters(new RSAParameters
            {
                Modulus = n,
                Exponent = e
            });

            keys.Add(new RsaSecurityKey(rsa)
            {
                KeyId = key.TryGetProperty("kid", out var kid)
                    ? kid.GetString()
                    : null
            });
        }

        // Actualiza la caché
        _cachedKeys = keys;
        _cacheExpiry = DateTime.UtcNow.Add(CacheDuration);

        return keys;
    }

    private static byte[] Base64UrlDecode(string base64Url)
    {
        var base64 = base64Url
            .Replace('-', '+')
            .Replace('_', '/');

        // Añade padding si falta
        base64 = (base64.Length % 4) switch
        {
            2 => base64 + "==",
            3 => base64 + "=",
            _ => base64
        };

        return Convert.FromBase64String(base64);
    }
}