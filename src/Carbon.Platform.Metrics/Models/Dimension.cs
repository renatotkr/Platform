using Carbon.Extensions;

namespace Carbon.Platform.Metrics
{
    public readonly struct Dimension
    {
        public Dimension(string name, long value)
            : this(name, value.ToString()) { }

        public Dimension(string name, string value)
        {
            Name  = name;
            Value = value;
        }

        public string Name { get; }

        public string Value { get; }
        
        public override string ToString()
        {
            return Name + "=" + Value;
        }

        public void Deconstruct(out string name, out string value)
        {
            name  = Name;
            value = Value;
        }

        // a=b
        public static Dimension Parse(string text)
        {
            var segments = text.Split(Seperators.Equal);

            return new Dimension(segments[0], segments[1]);
        }
    }
}

// AKA labels or tags