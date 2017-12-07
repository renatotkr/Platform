namespace Carbon.Platform.Metrics
{
    using static MetricType;

    public static class MetricNames
    {
        public static readonly Metric Compute      = new Metric(1, 1, "compute",      Delta, "nanoluna");
        public static readonly Metric Storage      = new Metric(1, 1, "storage",      Delta, "byte/hours");
        public static readonly Metric Transfer     = new Metric(1, 1, "transfer",     Delta, "bytes");
        public static readonly Metric Acceleration = new Metric(1, 1, "acceleration", Delta, "bytes");

        // { avg, max, min }

        // Environment Metrics --------------------------------------------------------------------------------------------------
        public static readonly Metric Exceptions             = new Metric(10, 1, "exceptions/count",         Delta, "count");            
        public static readonly Metric Requests               = new Metric(11, 1, "requests/count",           Delta, "count");       
        public static readonly Metric Connections            = new Metric(13, 1, "connection/count",         Delta, "count");

        // Computing Stats ---------------------------------------------------------------------------------------------------
        public static readonly Metric MemoryUsedBytes        = new Metric(21, 1, "memory/used:bytes",        Gauge, "byte");
        public static readonly Metric MemoryAvailableBytes   = new Metric(22, 1, "memory/available:bytes",   Gauge, "byte");
        public static readonly Metric MemoryTotalBytes       = new Metric(23, 1, "memory/total:bytes",       Gauge, "byte");
                                                                                                        
        // egress & ingress

        // dimensions: regionId, hostId, interfaceId
        public static readonly Metric NetworkReceivedBytes   = new Metric(31, 1, "network/received:bytes",   Delta, "byte");  
        public static readonly Metric NetworkSentBytes       = new Metric(32, 1, "network/sent:bytes",       Delta, "byte");  
        public static readonly Metric NetworkReceivedPackets = new Metric(33, 1, "network/received:packets", Delta, "count"); 
        public static readonly Metric NetworkSentPackets     = new Metric(34, 1, "network/sent:packets",     Delta, "count"); 
        public static readonly Metric NetworkDroppedPackets  = new Metric(35, 1, "network/dropped:packets",  Delta, "count"); 

        // network/ingress:bytes
        // network/ingress:packets
   
        // dimensions { regionId, hostId }
        public static readonly Metric ProcessorUserTime    = new Metric(41,   1, "processor/user:time",      Gauge, "%");
        public static readonly Metric ProcessorSystemTime  = new Metric(42,   1, "processor/system:time",    Gauge, "%");
        public static readonly Metric ProcessorIdleTime    = new Metric(43,   1, "processor/idle:time",      Gauge, "%");
        public static readonly Metric ProcessorStealTime   = new Metric(44,   1, "processor/steal:time",     Gauge, "%");

        // dimensions { regionId, hostId, volumeId }
        public static readonly Metric VolumeReadBytes        = new Metric(50, 1, "volume/read:bytes",        Delta, "byte");
        public static readonly Metric VolumeWriteBytes       = new Metric(51, 1, "volume/write:bytes",       Delta, "byte");
        public static readonly Metric VolumeTotalBytes       = new Metric(52, 1, "volume/total:bytes",       Gauge, "byte");
        public static readonly Metric VolumeAvailableBytes   = new Metric(53, 1, "volume/available:bytes",   Gauge, "byte");
        public static readonly Metric VolumeUsedBytes        = new Metric(54, 1, "volume/used:bytes",        Gauge, "byte");
        public static readonly Metric VolumeReadTime         = new Metric(55, 1, "volume/read:time",         Gauge, "%");
        public static readonly Metric VolumeWriteTime        = new Metric(56, 1, "volume/write:time",        Gauge, "%");
        public static readonly Metric VolumeReadOperations   = new Metric(57, 1, "volume/read:operations",   Delta, "count");
        public static readonly Metric VolumeWriteOperations  = new Metric(58, 1, "volume/write:operations",  Delta, "count");
    }
}

// exceptions,environmentId=1,hostId=100 value=1 ts

// When downsampling, the data will be agergated to meet the new policy


// metrics.borg.host

// https://cloud.google.com/monitoring/api/metrics#gcp-storage
