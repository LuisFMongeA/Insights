using Insights.AuthAPI.Services;

namespace Insights.AuthAPI.Extensions;

public static class AuthExtensions
{
    public static IServiceCollection AddAuthServices(this IServiceCollection services)
    {
        services.AddSingleton<RsaKeyService>();
        services.AddSingleton<IAuthService, AuthService>();
        return services;
    }
}