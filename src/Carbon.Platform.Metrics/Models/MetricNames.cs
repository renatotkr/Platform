namespace Carbon.Platform.Metrics
{
    using static MetricType;

    public static class MetricNames
    {
        public static readonly Metric Compute      = new Metric(0, "compute",      Delta, "nanoluna");
        public static readonly Metric Storage      = new Metric(0, "storage",      Delta, "byte/hours");
        public static readonly Metric Transfer     = new Metric(0, "transfer",     Delta, "bytes");
        public static readonly Metric Acceleration = new Metric(0, "acceleration", Delta, "bytes");

        // { avg, max, min }

        // Environment Metrics --------------------------------------------------------------------------------------------------
        public static readonly Metric Exceptions             = new Metric(10, "exceptions/count",         Delta, "count");            
        public static readonly Metric Requests               = new Metric(11, "requests/count",           Delta, "count");       
        public static readonly Metric Connections            = new Metric(13, "connection/count",         Delta, "count");

        // Computing Stats ---------------------------------------------------------------------------------------------------
        public static readonly Metric MemoryUsedBytes        = new Metric(21, "memory/used:bytes",        Gauge, "byte");
        public static readonly Metric MemoryAvailableBytes   = new Metric(22, "memory/available:bytes",   Gauge, "byte");
        public static readonly Metric MemoryTotalBytes       = new Metric(23, "memory/total:bytes",       Gauge, "byte");
                                                                                                        
        // egress & ingress

        // dimensions: regionId, hostId, interfaceId
        public static readonly Metric NetworkReceivedBytes   = new Metric(31, "network/received:bytes",   Delta, "byte");  
        public static readonly Metric NetworkSentBytes       = new Metric(32, "network/sent:bytes",       Delta, "byte");  
        public static readonly Metric NetworkReceivedPackets = new Metric(33, "network/received:packets", Delta, "count"); 
        public static readonly Metric NetworkSentPackets     = new Metric(34, "network/sent:packets",     Delta, "count"); 
        public static readonly Metric NetworkDroppedPackets  = new Metric(35, "network/dropped:packets",  Delta, "count"); 

        // network/ingress:bytes
        // network/ingress:packets
   
        // dimensions { regionId, hostId }
        public static readonly Metric ProcessorUserTime    = new Metric(41,   "processor/user:time",      Gauge, "%");
        public static readonly Metric ProcessorSystemTime  = new Metric(42,   "processor/system:time",    Gauge, "%");
        public static readonly Metric ProcessorIdleTime    = new Metric(43,   "processor/idle:time",      Gauge, "%");
        public static readonly Metric ProcessorStealTime   = new Metric(44,   "processor/steal:time",     Gauge, "%");

        // dimensions { regionId, hostId, volumeId }
        public static readonly Metric VolumeReadBytes        = new Metric(50, "volume/read:bytes",        Delta, "byte");
        public static readonly Metric VolumeWriteBytes       = new Metric(51, "volume/write:bytes",       Delta, "byte");
        public static readonly Metric VolumeTotalBytes       = new Metric(52, "volume/total:bytes",       Gauge, "byte");
        public static readonly Metric VolumeAvailableBytes   = new Metric(53, "volume/available:bytes",   Gauge, "byte");
        public static readonly Metric VolumeUsedBytes        = new Metric(54, "volume/used:bytes",        Gauge, "byte");
        public static readonly Metric VolumeReadTime         = new Metric(55, "volume/read:time",         Gauge, "%");
        public static readonly Metric VolumeWriteTime        = new Metric(56, "volume/write:time",        Gauge, "%");
        public static readonly Metric VolumeReadOperations   = new Metric(57, "volume/read:operations",   Delta, "count");
        public static readonly Metric VolumeWriteOperations  = new Metric(58, "volume/write:operations",  Delta, "count");
    }
}

// exceptions,environmentId=1,hostId=100 value=1 ts

// When downsampling, the data will be agergated to meet the new policy


// metrics.borg.host

// https://cloud.google.com/monitoring/api/metrics#gcp-storage
