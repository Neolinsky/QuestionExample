using Microsoft.AspNetCore.Mvc.Filters;

namespace Example;

public class TelemetryAttribute : ActionFilterAttribute
{
    public TelemetryAttribute(string requestId)
        => RequestId = requestId;

    public string RequestId { get; set; }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        context.HttpContext.Items["RequestId"] = RequestId;

        base.OnActionExecuting(context);
    }
}