using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Redbox.NetCore.Middleware.Abstractions;

namespace Redbox.NetCore.Middleware.Middleware
{
    public class GrafanaMetricsMiddleware
    {
        private readonly IGrafanaMetrics _metrics;

        private readonly RequestDelegate _next;

        public GrafanaMetricsMiddleware(RequestDelegate next, IGrafanaMetrics metrics)
        {
            _next = next;
            _metrics = metrics;
        }

        public async Task InvokeAsync(HttpContext context, IConfiguration configuration)
        {
            var requestPath = context.Request.Path.ToString();
            var responseTimer = new Stopwatch();
            responseTimer.Start();
            await _next(context);
            responseTimer.Stop();
            if (!(configuration.GetSection("AppMetrics:IgnoredRouteRegexPatterns").Get<List<string>>() ??
                  new List<string>()).Any(c => Regex.IsMatch(requestPath, c, RegexOptions.IgnoreCase)))
            {
                var action = GetAction(context);
                _metrics.CaptureResponseCode(requestPath, context.Request.Method, action,
                    context.Response.StatusCode.ToString());
                _metrics.CaptureResponseTime(context.Request.Path, context.Request.Method, action,
                    context.Response.StatusCode.ToString(), responseTimer.ElapsedMilliseconds);
            }
        }

        private static string GetAction(HttpContext context)
        {
            var routeData = context.GetRouteData();
            return string.Format("{0}.{1}", routeData.Values["controller"], routeData.Values["action"]);
        }
    }
}