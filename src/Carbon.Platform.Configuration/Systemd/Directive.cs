namespace Carbon.Platform.Configuration.Systemd
{
    public class Directive
    {
        public Directive(string name, string value, int order)
        {
            Name  = name;
            Value = value;
            Order = order;
        }

        public string Name { get; }

        public string Value { get; }

        public int Order { get; }
    }
}