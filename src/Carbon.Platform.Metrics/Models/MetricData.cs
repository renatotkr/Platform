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
          double value,
          long? timestamp) :
            this(name, dimensions, new[] { new MetricDataProperty("value", value) }, timestamp)
        { }

        public MetricData(
            string name,
            Dimension[] dimensions,
            MetricDataProperty[] properties,
            long? timestamp)
        {
            Name       = name ?? throw new ArgumentNullException(nameof(name));
            Dimensions = dimensions;
            Properties = properties ?? throw new ArgumentNullException(nameof(properties));
            Timestamp  = timestamp;
        }
        
        public readonly string Name;

        public readonly Dimension[] Dimensions; // aka labels

        public readonly MetricDataProperty[] Properties;

        public readonly long? Timestamp; // nanos since 1970
        
        public static MetricData Parse(string text)
        {
            var segments = text.Split(Seperators.Space);

            var keyValues = segments[0].Split(Seperators.Comma);

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
                name       : keyValues[0],
                dimensions : ParseDimensions(keyValues),
                properties : ParseProperties(segments[1].Split(Seperators.Comma)),
                timestamp  : timestamp
            );
        }

        private static Dimension[] ParseDimensions(string[] dimensionSegments)
        {
            if (dimensionSegments.Length == 1) return Array.Empty<Dimension>();

            var dimensions = new Dimension[dimensionSegments.Length - 1];

            for (var i = 0; i < dimensions.Length; i++)
            {
                dimensions[i] = Dimension.Parse(dimensionSegments[i + 1]);
            }

            return dimensions;
        }

        // values
        private static MetricDataProperty[] ParseProperties(string[] propertySegments)
        {
            var properties = new MetricDataProperty[propertySegments.Length];

            for (var i = 0; i < propertySegments.Length; i++)
            {
                properties[i] = MetricDataProperty.Parse(propertySegments[i]);
            }

            return properties;
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

            sb.Append(' ');

            for(var i = 0; i < Properties.Length; i++)
            {
                var property = Properties[i];

                if (i > 0) sb.Append(',');
                
                sb.Append(property.Name);
                sb.Append('=');
                sb.Append(MetricValue.Format(property.Value));
            }

            if (Timestamp != null)
            {
                sb.Append(' ');

                sb.Append(Timestamp.Value); // in nanos
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