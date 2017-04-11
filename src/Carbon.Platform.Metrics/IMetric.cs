namespace Carbon.Platform.Metrics
{
    public interface IMetric
    {
        long Id { get; }

        string Name { get; }

        string Unit { get; }
    }
}