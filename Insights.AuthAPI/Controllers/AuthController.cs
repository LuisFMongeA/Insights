using Insights.AuthAPI.Models;
using Insights.AuthAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace Insights.AuthAPI.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("token")]
    public async Task<ActionResult<TokenResponse>> Token()
    {
        // 1. Verifica que existe el header Authorization
        if (!Request.Headers.TryGetValue("Authorization", out var authHeader))
            return Unauthorized("Authorization header required");

        var authHeaderValue = authHeader.ToString();

        // 2. Verifica que es Basic Auth
        if (!authHeaderValue.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
            return Unauthorized("Basic authentication required");

        // 3. Decodifica Base64
        ClientCredentials? credentials;
        try
        {
            var base64 = authHeaderValue["Basic ".Length..].Trim();
            var decoded = Encoding.UTF8.GetString(Convert.FromBase64String(base64));

            // 4. Splitea por ":" — solo el primer ":" es separador
            // por si el secret contiene ":" 
            var separatorIndex = decoded.IndexOf(':');
            if (separatorIndex < 0)
                return Unauthorized("Invalid Basic Auth format");

            credentials = new ClientCredentials
            {
                ClientId = decoded[..separatorIndex],
                ClientSecret = decoded[(separatorIndex + 1)..]
            };
        }
        catch
        {
            return Unauthorized("Invalid Base64 encoding");
        }

        // 5. Valida y genera el token
        var token = await authService.GenerateTokenAsync(credentials);
        if (token is null)
            return Unauthorized("Invalid client credentials");

        return Ok(token);
    }

    [HttpPost("refresh")]
    public async Task<ActionResult<TokenResponse>> Refresh([FromBody] RefreshRequest request)
    {
        var token = await authService.RefreshTokenAsync(request.RefreshToken);

        if (token is null)
            return Unauthorized("Invalid or expired refresh token");

        return Ok(token);
    }
}