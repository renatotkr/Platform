using Carbon.Data.Annotations;

namespace Carbon.Platform.Metrics
{
    [Dataset("MetricRetentionPolicies")]
    public class MetricRetentionPolicy
    {
        public MetricRetentionPolicy(long metricId, string granularity, string duration)
        {
            MetricId    = metricId;
            Granularity = granularity;
            Duration    = duration;
        }

        [Member("metricId"), Key]
        public long MetricId { get; }

        [Member("granularity"), Key]  // e.g. PT1M, P1D, P1W, P1M, P1Y
        [Ascii, StringLength(20)]
        public string Granularity { get; }

        [Member("duration")]
        [Ascii, StringLength(20)]
        public string Duration { get; } // e.g. P1M, P1H, P1D
    }
    
}