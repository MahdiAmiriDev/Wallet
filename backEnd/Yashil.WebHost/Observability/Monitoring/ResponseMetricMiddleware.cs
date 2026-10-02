using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Yashil.WebHost.Observability.Monitoring;

public class ResponseMetricMiddleware
{
    private readonly RequestDelegate _request;

    public ResponseMetricMiddleware(RequestDelegate request)
    {
        _request = request ?? throw new ArgumentNullException(nameof(request));
    }

    public async Task Invoke(HttpContext httpContext, MetricReporter reporter)
    {
        var sw = Stopwatch.StartNew();

        try
        {
            await _request.Invoke(httpContext);
        }
        finally
        {
            sw.Stop();

            var controllerActionDescriptor = httpContext
                .GetEndpoint()
                ?.Metadata
                .GetMetadata<ControllerActionDescriptor>();

            var controllerName = controllerActionDescriptor?.ControllerName;
            var actionName = controllerActionDescriptor?.ActionName;

            var path = httpContext.Request.Path.Value;

            if (!string.IsNullOrEmpty(controllerName) && !string.IsNullOrEmpty(actionName))
            {
                path = $"{controllerName}/{actionName}";
            }
            else
            {
                controllerName = "resource";
                actionName = "resource";
            }
            reporter.RegisterRequest(path);
            reporter.RegisterResponseTime(httpContext.Response.StatusCode,
                httpContext.Request.Method, controllerName, actionName, path, httpContext.Request.Host.Value, sw.Elapsed);


        }
    }
}
