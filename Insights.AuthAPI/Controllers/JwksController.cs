using Insights.AuthAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace Insights.AuthAPI.Controllers;

[ApiController]
public class JwksController(RsaKeyService rsaKeyService) : ControllerBase
{
    [HttpGet(".well-known/jwks.json")]
    public IActionResult GetJwks()
    {
        var parameters = rsaKeyService.GetPublicKeyParameters();

        var jwks = new
        {
            keys = new[]
            {
                new
                {
                    kty = "RSA",
                    use = "sig",
                    kid = "insights-key-1",
                    alg = "RS256",
                    // n y e son los componentes matemáticos de la clave pública RSA
                    n = Base64UrlEncode(parameters.Modulus!),
                    e = Base64UrlEncode(parameters.Exponent!)
                }
            }
        };

        return Ok(jwks);
    }

    // JWKS usa Base64Url, que es distinto al Base64 estándar
    private static string Base64UrlEncode(byte[] bytes)
        => Convert.ToBase64String(bytes)
            .TrimEnd('=')
            .Replace('+', '-')
            .Replace('/', '_');
}