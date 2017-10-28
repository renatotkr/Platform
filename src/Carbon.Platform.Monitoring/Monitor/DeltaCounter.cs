using System;
using System.Collections.Generic;

using Carbon.Platform.Metrics;

namespace Carbon.Platform.Monitoring
{
    internal class DeltaValueMonitor : IMonitor
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

        public IEnumerable<MetricData> Observe()
        {
            var next = action();

            var delta = last - next;

            last = next;

            yield return new MetricData(metricName, dimensions, "count", delta, TimestampHelper.Get(DateTime.Now));
        }
        
        public void Dispose() { }
    }
}