using App.Metrics;

namespace Redbox.NetCore.Middleware.Middleware
{
    public static class KioskMetricsBuilder
    {
        public static IMetricsRoot Build(string metricsNamespace)
        {
            return AppMetrics.CreateDefaultBuilder().Configuration
                .Configure(options => options.DefaultContextLabel = metricsNamespace).OutputMetrics
                .AsPrometheusPlainText().Build();
        }
    }
}