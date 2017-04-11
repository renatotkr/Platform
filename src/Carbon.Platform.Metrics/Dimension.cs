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
    }
}

// Google calls these labels
// Time series db calls them tags
