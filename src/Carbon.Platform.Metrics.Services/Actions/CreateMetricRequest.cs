namespace Carbon.Platform.Metrics
{
    public class CreateMetricRequest
    {
        public CreateMetricRequest(
            string name,
            long ownerId,
            MetricType type,
            string unit,
            string[] dimensions = null)
        {
            Ensure.NotNullOrEmpty(name, nameof(name));
            Ensure.NotNullOrEmpty(unit, nameof(unit));

            Name       = name;
            OwnerId    = ownerId;
            Type       = type;
            Unit       = unit;
            Dimensions = dimensions;
        }

        public string Name { get; }

        public MetricType Type { get; }

        public string Unit { get; }

        public string[] Dimensions { get; }
        
        public long OwnerId { get; }
    }
}

// Metric       Dimensions        
// Storage      accountId, environmentId, Class
// Bandwidth    accountId, environmentId, Direction: (in/out), zone: "Americas"
// Compute      ???

// _____________________________________________________________________________