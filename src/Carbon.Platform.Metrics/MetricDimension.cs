using Carbon.Data.Annotations;

namespace Carbon.Platform.Metrics
{
    [Dataset("MetricDimensions")]
    public class MetricDimension
    {
        public MetricDimension(long metricId, string name, string dataType)
        {
            MetricId = metricId;
            Name     = name;
            DataType = dataType;
        }

        [Member("metricId"), Key]
        public long MetricId { get; }

        [Member("name"), Key]
        [StringLength(63)]
        public string Name { get; }

        [Member("dataType")] // Number, String
        [StringLength(30)]
        public string DataType { get; }
    }
}