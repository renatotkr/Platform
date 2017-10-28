using System;
using Carbon.Data.Annotations;

namespace Carbon.Platform.Metrics
{
    [Dataset("MetricDimensions")]
    public class MetricDimension
    {
        public MetricDimension() { }

        public MetricDimension(long id, string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Required", nameof(name));

            Id       = id;
            Name     = name;
        }

        [Member("id")]
        public long Id { get; } // metricId | #
        
        [Member("name"), Key]
        [Ascii, StringLength(100)]
        public string Name { get; }

        [Member("deleted")]
        public DateTime? Deleted { get; }

        [Member("created"), Timestamp]
        public DateTime Created { get; }
    }
}