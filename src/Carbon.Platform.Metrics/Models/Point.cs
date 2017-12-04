using System.Runtime.Serialization;

namespace Carbon.Platform.Metrics
{
    public readonly struct DataPoint
    {
        public DataPoint(long timestamp, double value)
        {
            Timestamp = timestamp;
            Value     = value;
        }

        [DataMember(Name = "timestamp", Order = 1)]
        public long Timestamp { get; }

        [DataMember(Name = "value", Order = 2)]
        public double Value { get; }
    }
}