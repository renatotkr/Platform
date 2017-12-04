namespace Carbon.Platform.Metrics
{
    public interface ITimeSeries
    {
        long Id { get; }

        string Name { get; }
    }
}