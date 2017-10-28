namespace Carbon.Platform.Metrics
{
    public enum MetricType : byte
    {
        Cumulative = 1, // Additive
        Delta      = 2, // Difference
        Gauge      = 3, // Discret Value
    }

    // Historogram
}