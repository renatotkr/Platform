using System;

using Carbon.Platform.Metrics;

namespace Carbon.Platform.Monitoring
{
    internal abstract class ResourceMonitor : IDisposable
    {
        public abstract MetricData[] Observe();

        public virtual void Dispose() { }
    }
}