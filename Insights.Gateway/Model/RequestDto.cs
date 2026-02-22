namespace Insights.Gateway.Model
{
    public class RequestDto
    {
        public string ApiType { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public object? Data { get; set; }
        public string AccessToken { get; set; } = string.Empty;
    }
}
