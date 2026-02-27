using Insights.AuthAPI.Models;

namespace Insights.AuthAPI.Services;

public interface IAuthService
{
    Task<TokenResponse?> GenerateTokenAsync(ClientCredentials credentials);
    Task<TokenResponse?> RefreshTokenAsync(string refreshToken);
}