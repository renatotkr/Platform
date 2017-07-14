using System;

using Carbon.Data.Annotations;
using Carbon.Data.Sequences;

namespace Carbon.Cloud.Logging
{
    [Dataset("RequestTimings", Schema = "Logs")]
    public class RequestTiming
    {
        public RequestTiming(
            Uid requestId,
            string name,
            TimeSpan start,
            TimeSpan duration)
        {
            RequestId = requestId;
            Name      = name ?? throw new ArgumentNullException(nameof(name));
            Start     = start;
            Duration  = duration;
        }

        [Member("requestId"), Key]
        public Uid RequestId { get; }

        // e.g. download, decode, encode, scale, upload
        [Member("name"), Key]
        [StringLength(50)]
        public string Name { get; }

        [Member("start")]
        [TimePrecision(TimePrecision.Millisecond)]
        public TimeSpan Start { get; }

        [Member("duration")]
        [TimePrecision(TimePrecision.Millisecond)]
        public TimeSpan Duration { get; }
    }
}

// Modeled after the Performance API
// https://w3c.github.io/user-timing/#dom-performance-measure
// https://developer.mozilla.org/en-US/docs/Web/API/PerformanceEntry