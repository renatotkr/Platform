using System;
using System.Runtime.Serialization;

namespace Carbon.Cloud.Logging
{
    public readonly struct Timing
    {
        public Timing(string name, TimeSpan start, TimeSpan duration)
        {
            Name     = name ?? throw new ArgumentNullException(nameof(name));
            Start    = start;
            Duration = duration;
        }

        [DataMember(Name = "name", Order = 1)]
        public string Name { get; }

        [DataMember(Name = "start", Order = 2, EmitDefaultValue = false)]
        public TimeSpan Start { get; }

        [DataMember(Name = "duration", Order = 3, EmitDefaultValue = false)]
        public TimeSpan Duration { get; }
    }
}