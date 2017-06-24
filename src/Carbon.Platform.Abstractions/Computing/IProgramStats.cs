namespace Carbon.Platform.Computing
{
    public interface IProgramStats
    {
        long ErrorCount { get; }

        long RequestCount { get; }

        long ComputeUnits { get; }
    }
}