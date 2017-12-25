using System;
using System.Runtime.Serialization;

namespace Carbon.Cloud.Logging
{
    public readonly struct Timing
    {
        public Timing(string name, TimeSpan start, TimeSpan duration)
        {
            Name     = name; // not null?
            Start    = start;
            Duration = duration;
        }

        [DataMember(Name = "name", Order = 1)]
        public readonly string Name;

        [DataMember(Name = "start", Order = 2, EmitDefaultValue = false)]
        public readonly TimeSpan Start;

        [DataMember(Name = "duration", Order = 3, EmitDefaultValue = false)]
        public readonly TimeSpan Duration;
    }
}