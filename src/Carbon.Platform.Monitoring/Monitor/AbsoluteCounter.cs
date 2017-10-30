﻿using System;
using System.Collections.Generic;

using Carbon.Platform.Metrics;
using Carbon.Time;

namespace Carbon.Platform.Monitoring
{
    internal class AbsoluteValueMonitor : IMonitor
    {
        private readonly IMetric metric;
        private readonly Dimension[] dimensions;
        private Func<double> action;

        public AbsoluteValueMonitor(IMetric metric, Dimension[] dimensions, Func<double> action)
        {
            this.metric = metric ?? throw new ArgumentNullException(nameof(metric));
            this.dimensions = dimensions;
            this.action     = action;
        }

        public IEnumerable<MetricData> Observe()
        {
            yield return new MetricData(metric.Name, dimensions, "count", action(), new Timestamp(DateTimeOffset.UtcNow).Value);
        }

        public void Dispose() { }
    }
}