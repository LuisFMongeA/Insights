namespace Insights.AuthAPI.Models;

public class TokenResponse
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public int ExpiresIn { get; set; }      // segundos hasta que expira el access token
    public string TokenType { get; set; } = "Basic";
}