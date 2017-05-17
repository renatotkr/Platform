using System;

namespace Carbon.Platform.Configuration
{
    public class ServiceUser
    {
        public static readonly ServiceUser WwwData = new ServiceUser("www-data");

        public ServiceUser(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public string Name { get; }

        public static ServiceUser Parse(string name)
        {
            return new ServiceUser(name);
        }

        public override string ToString() => Name;
    }
}