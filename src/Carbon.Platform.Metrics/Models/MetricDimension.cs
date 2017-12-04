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
            #region Preconditions

            if (id <= 0)
                throw new ArgumentException("Must be > 0", nameof(id));

            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Required", nameof(name));

            #endregion

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