using Carbon.Data.Annotations;

namespace Carbon.Platform.Metrics
{
    [Dataset("SeriesPoints")]
    public readonly struct SeriesPoint
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

// minute resolution: 1440 datapoints (34.56 kilobytes per day, 1MB/month, 12M/year)