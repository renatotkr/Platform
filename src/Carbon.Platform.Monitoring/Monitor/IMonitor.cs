using System;
using System.Collections.Generic;

using Carbon.Platform.Metrics;

namespace Carbon.Platform.Monitoring
{
    internal interface IMonitor : IDisposable
    {
        IEnumerable<MetricData> Observe();
    }
}