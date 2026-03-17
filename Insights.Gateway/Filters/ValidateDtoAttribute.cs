using Microsoft.AspNetCore.Mvc;


namespace Insights.Gateway.Filters;

public class ValidateDtoAttribute<T> : ServiceFilterAttribute where T : class
{
    public ValidateDtoAttribute() : base(typeof(ValidationFilter<T>))
    {
    }
}