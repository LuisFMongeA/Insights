using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Insights.Gateway.Filters;

public class ValidationFilter<T> : IAsyncActionFilter where T : class
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var param = context.ActionArguments.Values.OfType<T>().FirstOrDefault();
        if (param is null)
        {
            await next();
            return;
        }
        var validator = context.HttpContext.RequestServices
            .GetRequiredService<IValidator<T>>();

        var result = await validator.ValidateAsync(param);
        if (!result.IsValid) 
        {
            
            context.Result = new BadRequestObjectResult(
                result.Errors.Select(e => new
                {
                    field = e.PropertyName,
                    error = e.ErrorMessage
                }));
            return;
        }


        await next();
    }
}
