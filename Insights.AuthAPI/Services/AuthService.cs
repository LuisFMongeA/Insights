using Insights.AuthAPI.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Insights.AuthAPI.Services;

public class AuthService(IConfiguration configuration, RsaKeyService rsaKeyService) : IAuthService
{
    // Almacén en memoria de refresh tokens válidos
    // clave: refreshToken, valor: clientId
    private static readonly Dictionary<string, string> RefreshTokens = new();

    public Task<TokenResponse?> GenerateTokenAsync(ClientCredentials credentials)
    {
        // 1. Valida que el cliente existe y el secret es correcto
        var client = GetValidClient(credentials.ClientId, credentials.ClientSecret);
        if (client is null)
            return Task.FromResult<TokenResponse?>(null);

        // 2. Genera los tokens
        var tokenResponse = CreateTokenResponse(credentials.ClientId, client.Value.Scopes);
        return Task.FromResult<TokenResponse?>(tokenResponse);
    }

    public Task<TokenResponse?> RefreshTokenAsync(string refreshToken)
    {
        if (!RefreshTokens.TryGetValue(refreshToken, out var clientId))
            return Task.FromResult<TokenResponse?>(null);

        RefreshTokens.Remove(refreshToken);

        // Al refrescar recuperamos los scopes del cliente
        var clients = configuration.GetSection("Auth:Clients").GetChildren();
        var scopes = clients
            .Where(c => c["ClientId"] == clientId)
            .SelectMany(c => c.GetSection("Scopes").GetChildren())
            .Select(s => s.Value ?? string.Empty)
            .Where(s => !string.IsNullOrEmpty(s))
            .ToList();

        var tokenResponse = CreateTokenResponse(clientId, scopes);
        return Task.FromResult<TokenResponse?>(tokenResponse);
    }

    private TokenResponse CreateTokenResponse(string clientId, IEnumerable<string> scopes)
    {
        var accessToken = GenerateJwt(clientId, scopes);
        var refreshToken = GenerateRefreshToken();

        RefreshTokens[refreshToken] = clientId;

        return new TokenResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresIn = GetExpirationSeconds()
        };
    }

    private string GenerateJwt(string clientId, IEnumerable<string> scopes)
    {
        // Usa la clave privada RSA en lugar del shared secret
        var rsaSecurityKey = new RsaSecurityKey(rsaKeyService.GetPrivateKey())
        {
            KeyId = "insights-key-1"  // kid: identifica la clave en el JWKS
        };

        var credentials = new SigningCredentials(
            rsaSecurityKey,
            SecurityAlgorithms.RsaSha256);  // RS256 en lugar de HS256

        var claims = new List<Claim>
    {
        new(JwtRegisteredClaimNames.Sub, clientId),
        new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new(JwtRegisteredClaimNames.Iat,
            DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(),
            ClaimValueTypes.Integer64),
        new("client_id", clientId)
    };

        foreach (var scope in scopes)
            claims.Add(new Claim("scope", scope));

        var token = new JwtSecurityToken(
            issuer: configuration["Auth:Issuer"] ?? "insights-auth",
            audience: configuration["Auth:Audience"] ?? "insights-gateway",
            claims: claims,
            expires: DateTime.UtcNow.AddSeconds(GetExpirationSeconds()),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static string GenerateRefreshToken()
    {
        // Genera un token aleatorio criptográficamente seguro
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }

    private int GetExpirationSeconds()
        => int.TryParse(configuration["Auth:ExpiresInSeconds"], out var seconds)
            ? seconds
            : 3600; // 1 hora por defecto

    private (string ClientId, string Name, IEnumerable<string> Scopes)? GetValidClient(string clientId, string clientSecret)
    {
        // Lee los clientes desde configuration (user-secrets en desarrollo)
        var clients = configuration.GetSection("Auth:Clients").GetChildren();

        foreach (var client in clients)
        {
            var id = client["ClientId"];
            var secret = client["ClientSecret"];
            var name = client["Name"];
            var scopes = client.GetSection("Scopes")
           .GetChildren()
           .Select(s => s.Value ?? string.Empty)
           .Where(s => !string.IsNullOrEmpty(s))
           .ToList();

            if (id == clientId && CryptographicOperations.FixedTimeEquals(
                 Encoding.UTF8.GetBytes(secret ?? string.Empty),
                 Encoding.UTF8.GetBytes(clientSecret)))
                    return (id, name ?? id, scopes);
        }

        return null;
    }
}