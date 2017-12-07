using System;

using Carbon.Data.Annotations;

namespace Carbon.Platform.Metrics
{
    [Dataset("Series")]
    [UniqueIndex("name", "granularity")]
    public class Series : ITimeSeries
    {
        public Series() { }

        public Series(long id, string name, string granularity)
        {
            Validate.Id(id);
            Validate.NotNullOrEmpty(name, nameof(name));
            Validate.NotNullOrEmpty(granularity, nameof(granularity));

            Id          = id;
            Name        = name;
            Granularity = granularity;
        }

        [Member("id"), Key("seriesId", cacheSize: 100)]
        public long Id { get; }

        // request,country=US
        [Member("name")]
        [Ascii, StringLength(500)]
        public string Name { get; }

        // e.g. PT1M, PT5M, PT1H, P1D
        [Member("granularity")]
        [Ascii, StringLength(20)]
        public string Granularity { get; }

        // Dimensions?

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

// resolution (in seconds)

/*
A time series is a series of data points indexed (or listed or graphed) in time order. 
Most commonly, a time series is a sequence taken at successive equally spaced points in time. 
Thus it is a sequence of discrete-time data.
*/
