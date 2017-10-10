using System;
using System.Text;

namespace Carbon.Platform.Metrics
{
    // A point within a series (possible plotted across mutiple dimensions)
    
    public struct MetricData : IMetricData
    {
        private readonly IMetric metric;

        public MetricData(
            IMetric metric,
            Dimension[] dimensions, 
            double value, 
            DateTime timestamp)
        {
            this.metric = metric ?? throw new ArgumentNullException(nameof(metric));

            Dimensions = dimensions;
            Value      = value;
            Timestamp  = timestamp;
        }

        public string Name => metric.Name;

        public string Unit => metric.Unit;

        public Dimension[] Dimensions { get; }

        public double Value { get; }
        
        public DateTime Timestamp { get; }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append(Name);

            foreach (var dimension in Dimensions)
            {
                sb.Append(",");

                sb.Append(dimension.Name);
                sb.Append('=');
                sb.Append(dimension.Value);
            }

            sb.Append(" value=");

            sb.Append(Value);

            sb.Append(" ");

            var ts = new DateTimeOffset(Timestamp).ToUnixTimeMilliseconds() * 1000;

            sb.Append(ts); // in nanos

            // requestCount,appId=1,appVersion=5.1.1 value=1000 1422568543702900257

            return sb.ToString();
        }
    }
}