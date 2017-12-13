using System;
using System.Text;

using Carbon.Extensions;

namespace Carbon.Platform.Metrics
{
    public readonly struct MetricData
    {
        public MetricData(
            string name,
            Dimension[] dimensions,
            string unit,
            double value,
            long? timestamp)
        {
            Name       = name ?? throw new ArgumentNullException(nameof(name));
            Dimensions = dimensions;
            Unit       = unit;
            Value      = value;
            Timestamp  = timestamp;
        }

        public readonly string Name;

        public readonly Dimension[] Dimensions; // aka labels

        // Fields/ Properties?

        public readonly string Unit;

        public readonly double Value;

        public readonly long? Timestamp; // nanos since 1970
        
        public static MetricData Parse(string text)
        {
            var segments = text.Split(Seperators.Space);

            var labels        = segments[0].Split(Seperators.Comma);
            var dimensions    = ParseLabels(labels);
            var (unit, value) = ParseFields(segments[1].Split(Seperators.Comma));

            long? timestamp;

            if (segments.Length == 3)
            {
                timestamp = long.Parse(segments[2]);
            }
            else
            {
                timestamp = null;
            }

            return new MetricData(
                name       : labels[0],
                dimensions : ParseLabels(segments[0].Split(Seperators.Comma)),
                unit       : unit,
                value      : value,
                timestamp  : timestamp
            );
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

        // values
        private static (string unit, double value) ParseFields(string[] fields)
        {
            string unit = null;
            double value = 0;

            for (var i = 0; i < fields.Length; i++)
            {
                var field = Dimension.Parse(fields[i]);

                switch (field.Name)
                {
                    case "unit"  : unit = field.Value.ToString();         break;
                    case "value" : value = Convert.ToDouble(field.Value); break;
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
                    sb.Append(',');

                    sb.Append(dimension.Name);
                    sb.Append('=');
                    sb.Append(dimension.Value);
                }
            }
            
            sb.Append(" value=");

            sb.Append(Value);

            if (Timestamp != null)
            {
                sb.Append(' ');

                sb.Append((long)Timestamp.Value); // in nanos
            }

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