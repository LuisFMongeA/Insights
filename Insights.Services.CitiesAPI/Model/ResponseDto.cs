namespace Insights.CitiesAPI.Model
{
    public class ResponseDto
    {
        public object? Result { get; set; }
        public bool IsSuccess { get; set; } = true;
        public string ErrorMessages { get; set; } = string.Empty;
    }
}
