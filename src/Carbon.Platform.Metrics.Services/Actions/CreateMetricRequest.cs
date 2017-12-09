using System;

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
            #region Preconditions

            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Required", nameof(name));
            }

            if (string.IsNullOrEmpty(unit))
            {
                throw new ArgumentException("Required", nameof(unit));
            }

            #endregion

            Name       = name;
            OwnerId    = ownerId;
            Type       = type;
            Unit       = unit ?? throw new ArgumentNullException(nameof(unit));
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