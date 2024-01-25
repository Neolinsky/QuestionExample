using Prometheus;

namespace Example;

public class ApiMetricsMiddleware
{
    private readonly RequestDelegate _next;

    public ApiMetricsMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        var exampleMetricHistogram = Metrics.CreateHistogram("api_calls_histogram", "displays the api calls information", new HistogramConfiguration { LabelNames = new[] { "api_name" } });
        
        var apiName = httpContext.Items["RequestId"] as string ?? httpContext.Request.Path;

        using (exampleMetricHistogram.WithLabels(apiName).NewTimer()) // now it will not get httpContext.Items["RequestId"] because ActionFilterAttribute will only be called in _next(httpContext);
        {
            await _next(httpContext);
        }
        
        apiName = httpContext.Items["RequestId"] as string ?? httpContext.Request.Path; // and here i would get what i want.
    }
}