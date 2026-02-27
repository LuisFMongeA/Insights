using System.Security.Cryptography;

namespace Insights.AuthAPI.Services;

public class RsaKeyService : IDisposable
{
    private readonly RSA _rsa;

    public RsaKeyService(IConfiguration configuration)
    {
        _rsa = RSA.Create();

        var privateKeyBase64 = configuration["Auth:RsaPrivateKey"]
            ?? throw new InvalidOperationException("Auth:RsaPrivateKey not configured");

        var privateKeyBytes = Convert.FromBase64String(privateKeyBase64);
        _rsa.ImportRSAPrivateKey(privateKeyBytes, out _);
    }

    // Para firmar JWT
    public RSA GetPrivateKey() => _rsa;

    // Para exponer en JWKS
    public RSAParameters GetPublicKeyParameters() => _rsa.ExportParameters(false);

    public void Dispose() => _rsa.Dispose();
}