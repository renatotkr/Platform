using System;

namespace Carbon.Platform.Configuration.Systemd
{
    public readonly struct Directive
    {
        public Directive(string name, string value, int order)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Value = value;
            Order = order;
        }

        public readonly string Name;

        public readonly string Value;

        public readonly int Order;
    }
}