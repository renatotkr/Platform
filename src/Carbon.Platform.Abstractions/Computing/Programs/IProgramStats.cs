namespace Carbon.Platform.Computing
{
    public interface IProgramStats
    {
        long ErrorCount { get; }

        long RequestCount { get; }

        decimal ComputeUnits { get; }
    }
}