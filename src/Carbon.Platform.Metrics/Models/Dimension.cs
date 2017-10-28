namespace Carbon.Platform.Metrics
{
    public struct Dimension
    {
        public Dimension(string name, long value)
            : this(name, value.ToString()) { }

        public Dimension(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; }

        public string Value { get; }
        
        public override string ToString()
        {
            return Name + "=" + Value;
        }

        // a=b
        public static Dimension Parse(string text)
        {
            var segments = text.Split('=');

            return new Dimension(segments[0], segments[1]);
        }
    }
}

// Google calls these labels
// Time series db calls them tags
