namespace Carbon.Platform.Metrics
{
    using static MetricType;

    public static class MetricNames
    {
        // { avg, max, min }

        // Environment Stats --------------------------------------------------------------------------------------------------
        public static readonly Metric Exceptions             = new Metric(10, "exceptions",               Cumulative, "count");            
        public static readonly Metric Requests               = new Metric(11, "requests",                 Cumulative, "count");       
        public static readonly Metric Connections            = new Metric(13, "connections",              Cumulative, "count");

        // Computing Stats ---------------------------------------------------------------------------------------------------
        public static readonly Metric MemoryUsedBytes        = new Metric(21, "memory/used:bytes",        Cumulative, "byte");
        public static readonly Metric MemoryAvailableBytes   = new Metric(22, "memory/available:bytes",   Cumulative, "byte");
        public static readonly Metric MemoryTotalBytes       = new Metric(23, "memory/total:bytes",       Cumulative, "byte");
                                                                                                
        // network:transfer
        
        // egress & ingress

        // dimensions: regionId, hostId, interfaceId
        public static readonly Metric NetworkReceivedBytes   = new Metric(31, "network/received:bytes",   Cumulative, "byte");  
        public static readonly Metric NetworkSentBytes       = new Metric(32, "network/sent:bytes",       Cumulative, "byte");  
        public static readonly Metric NetworkReceivedPackets = new Metric(33, "network/received:packets", Cumulative, "count"); 
        public static readonly Metric NetworkSentPackets     = new Metric(34, "network/sent:packets",     Cumulative, "count"); 
        public static readonly Metric NetworkDroppedPackets  = new Metric(35, "network/dropped:packets",  Cumulative, "count"); 

   
        // dimensions { regionId, hostId }
        public static readonly Metric ProcessorUserTime    = new Metric(41,   "processor/user:time",         MetricType.Gauge, "%");
        public static readonly Metric ProcessorSystemTime  = new Metric(42,   "processor/system:time",       MetricType.Gauge, "%");
        public static readonly Metric ProcessorIdleTime    = new Metric(43,   "processor/idle:time",         MetricType.Gauge, "%");
        public static readonly Metric ProcessorStealTime   = new Metric(44,   "processor/steal:time",        MetricType.Gauge, "%");

        // dimensions { regionId, hostId, volumeId }
        public static readonly Metric VolumeReadBytes        = new Metric(50, "volume/read:bytes",        Cumulative, "byte");
        public static readonly Metric VolumeWriteBytes       = new Metric(51, "volume/write:bytes",       Cumulative, "byte");
        public static readonly Metric VolumeTotalBytes       = new Metric(52, "volume/total:bytes",       MetricType.Gauge, "byte");
        public static readonly Metric VolumeAvailableBytes   = new Metric(53, "volume/available:bytes",   MetricType.Gauge, "byte");
        public static readonly Metric VolumeUsedBytes        = new Metric(54, "volume/used:bytes",        MetricType.Gauge, "byte");
        public static readonly Metric VolumeReadTime         = new Metric(55, "volume/read:time",         MetricType.Gauge, "%");
        public static readonly Metric VolumeWriteTime        = new Metric(56, "volume/write:time",        MetricType.Gauge, "%");
        public static readonly Metric VolumeReadOperations   = new Metric(57, "volume/read:operations",   Cumulative, "count");
        public static readonly Metric VolumeWriteOperations  = new Metric(58, "volume/write:operations",  Cumulative, "count");
    }
}

// exceptions,environmentId=1,hostId=100 value=1 ts

// When downsampling, the data will be agergated to meet the new policy


// metrics.borg.host

// https://cloud.google.com/monitoring/api/metrics#gcp-storage
