namespace Carbon.Platform.Metrics
{
    public interface IMetricData
    {
        string Name { get; }
        
        Dimension[] Dimensions { get; }

        double Value { get; }
    }
}