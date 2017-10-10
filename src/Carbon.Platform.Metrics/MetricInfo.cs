using System;
using Carbon.Data.Annotations;

namespace Carbon.Platform.Metrics
{
    [Dataset("Metrics")]
    public class MetricInfo : IMetric
    {
        public MetricInfo(long id, string name, string unit)
        {
            Id = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Unit = unit;
        }

        [Member("id"), Key]
        public long Id { get; }

        // [Member("namespace")]
        // public string Namespace { get; set; }

        [Member("name"), Unique]
        [StringLength(100)]
        public string Name { get; }

        // Float64, Int64
        // public string DataType { get; set; }

        // bytes
        // bytes/second
        [Member("unit")]
        [StringLength(50)]
        public string Unit { get; }  
    }

    /*
    [Dataset("MetricRetentionPolicies")]
    public class MetricRetentionPolicy
    {
        public MetricRetentionPolicy(long metricId, string period, string duration)
        {
            MetricId = metricId;
            Period = period;
            Duration = duration;
        }

        [Member("metricId"), Key]
        public long MetricId { get; }

        [Member("period"), Key]              // e.g. P1D, P1W, P1M, P1Y
        public string Period { get; }

        [Member("duration")]
        public string Duration { get; } // e.g. P1M, P1H, P1D
    }
    */
}