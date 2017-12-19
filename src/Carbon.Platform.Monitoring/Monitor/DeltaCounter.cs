using System;

using Carbon.Platform.Metrics;
using Carbon.Time;

namespace Carbon.Platform.Monitoring
{
    internal sealed class DeltaValueMonitor : ResourceMonitor
    {
        private readonly Dimension[] dimensions;
        private Func<double> action;
        private readonly string metricName;

        public DeltaValueMonitor(string metricName, Dimension[] dimensions, Func<double> action)
        {
            this.metricName = metricName ?? throw new ArgumentNullException(nameof(metricName));
            this.dimensions = dimensions ?? throw new ArgumentNullException(nameof(dimensions));
            this.action     = action     ?? throw new ArgumentNullException(nameof(action));

            last = action();
        }

        double last;

        public override MetricData[] Observe()
        {
            var current = action();

            var delta = last - current;

            last = current;

            return new[] {
                new MetricData(metricName, dimensions, delta, new Timestamp(DateTime.UtcNow))
            };
        }        
    }
}