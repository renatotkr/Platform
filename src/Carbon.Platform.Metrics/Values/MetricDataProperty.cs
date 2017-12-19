
using System;
using Carbon.Extensions;

namespace Carbon.Platform.Metrics
{
    public readonly struct MetricDataProperty
    {
        public MetricDataProperty(string name, double value)
        {
            Name = name;
            Value = value; 
        }

        public MetricDataProperty(string name, long value)
        {
            Name  = name;
            Value = value; 
        }

        public MetricDataProperty(string name, TimeSpan value)
        {
            Name = name;
            Value = (double)value.Ticks / TimeSpan.TicksPerSecond;
        }

        public MetricDataProperty(string name, bool value)
        {
            Name  = name;
            Value = value;
        }

        public MetricDataProperty(string name, string value)
        {
            Name = name;
            Value = value;
        }

        private MetricDataProperty(string name, object value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; }

        // String, Boolean, String, Double, Int64
        public object Value { get; }
        
        // a=b
        public static MetricDataProperty Parse(string text)
        {
            var segments = text.Split(Seperators.Equal);

            return new MetricDataProperty(segments[0], MetricValue.Parse(segments[1]));
        }
    }
}