using System;

namespace Carbon.Platform.Configuration.Systemd
{
    public struct Directive
    {
        public Directive(string name, string value, int order)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Value = value;
            Order = order;
        }

        public string Name { get; }

        public string Value { get; }

        public int Order { get; }
    }
}