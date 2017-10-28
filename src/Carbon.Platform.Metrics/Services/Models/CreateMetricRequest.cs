using System;

namespace Carbon.Platform.Metrics
{
    public class CreateMetricRequest
    {
        public CreateMetricRequest(
            string name,
            MetricType type,
            string unit,
            string[] dimensions)
        {
            Name       = name ?? throw new ArgumentNullException(nameof(name));
            Type       = type;
            Unit       = unit ?? throw new ArgumentNullException(nameof(unit));
            Dimensions = dimensions;
        }

        public string Name { get; }

        public MetricType Type { get; }

        public string Unit { get; }

        public string[] Dimensions { get; }
    }
}

// Metric       Dimensions        
// Storage      accountId, environmentId, Class
// Bandwidth    accountId, environmentId, Direction: (in/out), zone: "Americas"
// Compute      ???

// _____________________________________________________________________________