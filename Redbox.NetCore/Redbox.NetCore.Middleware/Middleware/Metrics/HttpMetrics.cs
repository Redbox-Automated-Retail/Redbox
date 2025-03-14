using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Metrics;
using Microsoft.Extensions.Logging;
using Redbox.NetCore.Logging.Extensions;
using Redbox.NetCore.Middleware.Abstractions;
using Redbox.NetCore.Middleware.Extensions;

namespace Redbox.NetCore.Middleware.Metrics
{
    public class HttpMetrics : ICommonMetrics
    {
        private readonly ILogger<HttpMetrics> _logger;
        private readonly IMetrics _metrics;

        public HttpMetrics(IMetrics metrics, ILogger<HttpMetrics> logger)
        {
            _metrics = metrics;
            _logger = logger;
        }

        public async Task CaptureMetrics(
            Dictionary<string, string> metricTagCollection,
            long elapsedMilliseconds)
        {
            await Task.Run(() =>
            {
                if (_metrics == null)
                    _logger.LogDebugWithSource("No implementation of IMetrics found. Skipping capture.",
                        "/var/lib/jenkins/workspace/a_common_redbox.netcore.middleware/Redbox.NetCore.Middleware/Metrics/HttpMetrics.cs");
                else
                    try
                    {
                        var tags = new MetricTags(metricTagCollection.Keys.ToArray(),
                            metricTagCollection.Values.ToArray());
                        _logger.LogInfoWithSource(string.Format("{0} took {1}ms", tags.ToJson(), elapsedMilliseconds),
                            "/var/lib/jenkins/workspace/a_common_redbox.netcore.middleware/Redbox.NetCore.Middleware/Metrics/HttpMetrics.cs");
                        _metrics.Measure.Counter.Increment(MetricsRegistry.ExternalRequests, tags);
                        _metrics.Measure.Histogram.Update(MetricsRegistry.ExternalResponseTimes, tags,
                            elapsedMilliseconds);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogErrorWithSource(ex, "Unhandled exception while capturing metrics",
                            "/var/lib/jenkins/workspace/a_common_redbox.netcore.middleware/Redbox.NetCore.Middleware/Metrics/HttpMetrics.cs");
                    }
            });
        }
    }
}