#if NET461
using System;
using System.Collections.Generic;
using System.Diagnostics;

using Carbon.Platform.Metrics;

namespace Carbon.Platform.Monitoring
{
    public sealed class WindowsMonitor : ResourceMonitor
    {
        private readonly IMetric metric;
        private readonly Dimension[] dimensions;

        private PerformanceCounter counter;
        private CounterSample last;

        public WindowsMonitor(IMetric metric, Dimension[] dimensions, PerformanceCounter counter)
        {
            this.metric         = metric ?? throw new ArgumentNullException(nameof(metric));
            this.counter        = counter ?? throw new ArgumentNullException(nameof(counter));
            this.dimensions     = dimensions ?? throw new ArgumentNullException(nameof(dimensions));

            try
            {
                last = counter.NextSample();
            }
            catch(Exception ex)
            {
                ThrowSampleError(ex);
            }
        }

        public override MetricData[] Observe()
        {
            float result = 0;

            try
            {
                var next = counter.NextSample();

                result = CounterSample.Calculate(last, next);

                this.last = next;
            }
            catch (Exception ex)
            {
                ThrowSampleError(ex);
            }

            return new[] { new MetricData(metric, dimensions, result, DateTime.UtcNow) };
        }

        public void ThrowSampleError(Exception ex)
        {
            var counterDescription = $"{counter.CategoryName},{counter.CounterName},{counter.InstanceName}";

            throw new Exception($"Error sampling {metric.Name} | {counterDescription}", ex);
        }

        public override void Dispose()
        {
            this.counter.Dispose();
        }
    }
}
#endif