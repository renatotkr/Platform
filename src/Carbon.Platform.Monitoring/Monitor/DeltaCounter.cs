using System;
using System.Collections.Generic;

using Carbon.Platform.Metrics;

namespace Carbon.Platform.Monitoring
{
    internal class DeltaValueMonitor : IMonitor
    {
        private readonly Dimension[] dimensions;
        private Func<double> action;
        private readonly IMetric metric;

        public DeltaValueMonitor(IMetric metric, Dimension[] dimensions, Func<double> action)
        {
            this.metric     = metric     ?? throw new ArgumentNullException(nameof(metric));
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

            yield return new MetricData(metric, dimensions, delta, DateTime.UtcNow);
        }
        
        public void Dispose() { }
    }
}