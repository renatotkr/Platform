﻿using System;
using Carbon.Data.Annotations;

namespace Carbon.Platform.Metrics
{
    [Dataset("Metrics")]
    public class Metric : IMetric
    {
        public Metric() { }

        public Metric(long id, string name, MetricType type, string unit)
        {
            #region Preconditions

            if (id <= 0)
                throw new ArgumentException("Must be > 0", nameof(id));

            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Required", nameof(name));

            if (string.IsNullOrEmpty(unit))
                throw new ArgumentException("Required", nameof(unit));

            #endregion

            Id    = id;
            Type  = type;
            Name  = name;
            Unit  = unit;
        }

        [Member("id"), Key("metricId")]
        public long Id { get; }

        [Member("name"), Unique]
        [Ascii, StringLength(100)]
        public string Name { get; }
        
        [Member("type")]
        public MetricType Type { get; }

        [Member("unit")]
        [StringLength(50)]
        public string Unit { get; }

        [Member("created")]
        public DateTime Created { get; }

        [Member("deleted")]
        public DateTime? Deleted { get;  }

        // OwnerId?
    }
}