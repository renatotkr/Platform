using System;
using Carbon.Data.Annotations;

namespace Carbon.Platform.Metrics
{
    [Dataset("Metrics")]
    [UniqueIndex("ownerId", "name")]
    public class Metric : IMetric
    {
        public Metric() { }

        public Metric(long id, long ownerId, string name, MetricType type, string unit)
        {
            Validate.Id(id);
            Validate.Id(ownerId, nameof(ownerId));
            Validate.NotNullOrEmpty(name, nameof(name));
            Validate.NotNull(unit, nameof(unit));

            if (name.Length > 100)
                throw new ArgumentException("May not exceed 100 characters", nameof(name));

            Id      = id;
            OwnerId = ownerId;
            Type    = type;
            Name    = name;
            Unit    = unit;
        }

        [Member("id"), Key("metricId")]
        public long Id { get; }

        [Member("name")]
        [Ascii, StringLength(100)]
        public string Name { get; }

        [Member("type")]
        public MetricType Type { get; }

        [Member("unit")]
        [StringLength(50)]
        public string Unit { get; }

        [Member("ownerId"), Indexed]
        public long OwnerId { get; }

        [Member("created")]
        public DateTime Created { get; }

        [Member("deleted")]
        public DateTime? Deleted { get; }
    }
}