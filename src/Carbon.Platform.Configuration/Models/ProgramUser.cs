using System;

namespace Carbon.Platform.Configuration
{
    public class ProgramUser
    {
        public static readonly ProgramUser WwwData = new ProgramUser("www-data");

        public ProgramUser(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public string Name { get; }

        public static ProgramUser Parse(string name)
        {
            return new ProgramUser(name);
        }

        public override string ToString() => Name;
    }
}