using System;

using Carbon.Platform.Metrics;
using Carbon.Time;

namespace Carbon.Platform.Monitoring
{
    internal class AbsoluteValueMonitor : ResourceMonitor
    {
        private readonly IMetric metric;
        private readonly Dimension[] dimensions;
        private Func<double> action;

        public AbsoluteValueMonitor(IMetric metric, Dimension[] dimensions, Func<double> action)
        {
            this.metric     = metric ?? throw new ArgumentNullException(nameof(metric));
            this.dimensions = dimensions;
            this.action     = action;
        }

        public override MetricData[] Observe()
        {
            var value = action();

            return new[] {
                new MetricData(metric.Name, dimensions, "count", value, new Timestamp(DateTimeOffset.UtcNow).Value)
            };
        }
    }
}