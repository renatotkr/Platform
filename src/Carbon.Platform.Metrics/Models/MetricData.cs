﻿using System;
using System.Text;

namespace Carbon.Platform.Metrics
{
    public struct MetricData
    {
        public MetricData(
            string name,
            Dimension[] dimensions,
            string unit,
            double value,
            long timestamp)
        {
            Name       = name;
            Dimensions = dimensions;
            Unit       = unit;
            Value      = value;
            Timestamp  = timestamp;
        }

        public string Name { get; }
        
        public Dimension[] Dimensions { get; } // aka labels
       
        public string Unit { get; }

        public double Value { get; }
        
        public long Timestamp { get; } // nanos since 1970
        
        public static MetricData Parse(string text)
        {
            var segments = text.Split(' ');

            var labels     = segments[0].Split(',');
            var dimensions = ParseLabels(labels);
            var fields     = ParseFields(segments[1].Split(','));
          
            return new MetricData(
                name        : labels[0],
                dimensions  : ParseLabels(segments[0].Split(',')),
                unit        : fields.unit,
                value       : fields.value,
                timestamp   : long.Parse(segments[2]));
        }

        private static Dimension[] ParseLabels(string[] labels)
        {
            if (labels.Length == 1) return Array.Empty<Dimension>();

            var dimensions = new Dimension[labels.Length - 1];

            for (var i = 0; i < dimensions.Length; i++)
            {
                dimensions[i] = Dimension.Parse(labels[i + 1]);
            }

            return dimensions;
        }

        // Values
        private static (string unit, double value) ParseFields(string[] fields)
        {
            string unit = null;
            double value = 0;

            for (var i = 0; i < fields.Length; i++)
            {
                var field = Dimension.Parse(fields[i]);

                switch (field.Name)
                {
                    case "unit"  : unit = field.Value; break;
                    case "value" : value = double.Parse(field.Value); break;
                }

            }

            return (unit, value);
        }


        public void WriteTo(StringBuilder sb)
        {
            sb.Append(Name);

            if (Dimensions != null)
            {
                foreach (var dimension in Dimensions)
                {
                    sb.Append(",");

                    sb.Append(dimension.Name);
                    sb.Append("=");
                    sb.Append(dimension.Value);
                }
            }
            
            sb.Append(" value=");

            sb.Append(Value);

            sb.Append(" ");

            sb.Append(Timestamp); // in nanos

            // requestCount,appId=1,appVersion=5.1.1 value=1000 1422568543702900257
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            
            WriteTo(sb);

            return sb.ToString();
        }
    }
}

// storage:bytes
// storage:object
// storage:byte/hours
// transfer:bytes
 
// A point within a series (possible plotted across mutiple dimensions)

// we report the delta

// computeUnits accountId=1 value=100 1235234

// figure out what bucket it belongs in... 