using Carbon.Data.Annotations;

namespace Carbon.Platform.Metrics
{
    [Dataset("SeriesPoints")]
    public struct SeriesPoint
    {
        public SeriesPoint(long seriesId, long timestamp, double value)
        {
            SeriesId  = seriesId;
            Timestamp = timestamp;
            Value     = value;
        }

        [Member("seriesId"), Key]
        public long SeriesId { get; }

        [Member("timestamp"), Key] // nanos since 1970
        public long Timestamp { get; }
        
        [Member("value")]
        public double Value { get; }
    }
}


// 50GB transfer
// 45629 operations
// 5mbs
// 50%