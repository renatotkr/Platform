using System;

using Carbon.Data.Annotations;

namespace Carbon.Platform.Metrics
{
    [Dataset("Series")]
    [UniqueIndex("name", "granularity")]
    public class Series : ISeries
    {
        public Series() { }

        public Series(long id, string name, string granularity)
        {
            if (id <= 0)
                throw new ArgumentException("Must be > 0", nameof(id));

            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Required", nameof(name));

            Id          = id;
            Name        = name;
            Granularity = granularity ?? throw new ArgumentNullException(nameof(granularity));
        }

        [Member("id"), Key("seriesId", cacheSize: 100)]
        public long Id { get; }

        // request,country=US
        [Member("name")]
        [Ascii, StringLength(500)]
        public string Name { get; }

        // e.g. PT1M, PT5M, PT1H, P1D
        [Member("granularity")]
        [Ascii, StringLength(20)] // resolution (in seconds)
        public string Granularity { get; }

        // 31,536,000 seconds in a year

        [Member("created"), Timestamp]
        public DateTime Created { get; }
    }

    // 12:00:00 AM => 21.61 
    // 12:30:00 AM => 21.64
    // 1:00:00 AM  => 21.86 

    // 1 minute grandularity in redis?

    // bandwidth:egress,country=US
    // bandwidth:ingress,accountId=1,country=AU 
    // storage,accountId:1  
    // bandwidth,accountId=1,type:egress,country=AU 
    // compute,accountId=10 value=50

    // compute          | unit
    // storage          | GB/hour
    // transfer:ingress | GB
    // transfer:egress  | GB
    // acceleration     | ???
}


/*
A time series is a series of data points indexed (or listed or graphed) in time order. 
Most commonly, a time series is a sequence taken at successive equally spaced points in time. 
Thus it is a sequence of discrete-time data.
*/
