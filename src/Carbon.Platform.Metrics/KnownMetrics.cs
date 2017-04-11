namespace Carbon.Platform.Metrics
{
    public static class KnownMetrics
    {
        // dimensions { regionId, hostId, appId, appVersion }
        public static MetricInfo AppExceptions          = new MetricInfo(10, "app/exceptions",           "count");            
        public static MetricInfo AppRequests            = new MetricInfo(11, "app/requests",             "count");       
        public static MetricInfo AppProcessTime         = new MetricInfo(12, "app/processTime",          "%");               
        public static MetricInfo AppActiveConnections   = new MetricInfo(13, "app/activeConnections",    "count"); // { avg, max, min }
        public static MetricInfo AppTotalConnections    = new MetricInfo(14, "app/totalConnections",     "count");

        // dimensions  { regionId, hostId }
        public static MetricInfo MemoryUsedBytes        = new MetricInfo(21, "memory/usedBytes",        "byte");
        public static MetricInfo MemoryAvailableBytes   = new MetricInfo(22, "memory/availableBytes",   "byte");
        public static MetricInfo MemoryTotalBytes       = new MetricInfo(23, "memory/totalBytes",       "byte");

        // dimensions: regionId, hostId, interfaceId
        public static MetricInfo NetworkReceivedBytes   = new MetricInfo(31, "network/receivedBytes",   "byte");
        public static MetricInfo NetworkSentBytes       = new MetricInfo(32, "network/sentBytes",       "byte");
        public static MetricInfo NetworkReceivedPackets = new MetricInfo(33, "network/receivedPackets", "count");
        public static MetricInfo NetworkSentPackets     = new MetricInfo(34, "network/sentPackets",     "count");
        public static MetricInfo NetworkDroppedPackets  = new MetricInfo(35, "network/droppedPackets",  "count");

        // dimensions { regionId, hostId }
        public static MetricInfo ProcessorUserTime    = new MetricInfo(41, "processor/userTime",         "%");
        public static MetricInfo ProcessorSystemTime  = new MetricInfo(42, "processor/systemTime",       "%");
        public static MetricInfo ProcessorIdleTime    = new MetricInfo(43, "processor/idleTime",         "%");
        public static MetricInfo ProcessorStealTime   = new MetricInfo(44, "processor/stealTime",        "%");

        // dimensions { regionId, hostId, volumeId }
        public static MetricInfo VolumeReadBytes        = new MetricInfo(50, "volume/readBytes",        "byte");
        public static MetricInfo VolumeWriteBytes       = new MetricInfo(51, "volume/writeBytes",       "byte");
        public static MetricInfo VolumeTotalBytes       = new MetricInfo(52, "volume/totalBytes",       "byte");
        public static MetricInfo VolumeAvailableBytes   = new MetricInfo(53, "volume/availableBytes",   "byte");
        public static MetricInfo VolumeUsedBytes        = new MetricInfo(54, "volume/usedBytes",        "byte");
        public static MetricInfo VolumeReadTime         = new MetricInfo(55, "volume/readTime",         "%");
        public static MetricInfo VolumeWriteTime        = new MetricInfo(56, "volume/writeTime",        "%");
        public static MetricInfo VolumeReadOperations   = new MetricInfo(57, "volume/readOperations",   "count");
        public static MetricInfo VolumeWriteOperations  = new MetricInfo(58, "volume/writeOperations",  "count");
    }
}

// app/exceptions,regionId=1000,hostId=100,appId=1,appVersion=1.0.0 value=1 timeStampInNanoseconds

// When downsampling, the data will be agergated to meet the new policy


// https://cloud.google.com/monitoring/api/metrics#gcp-storage
