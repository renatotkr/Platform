namespace Carbon.Platform.Apps
{
    public interface IAppStats
    {
        long ErrorCount { get; }

        long RequestCount { get; }

        // decimal ComputeUnits { get; }
    }
}
