using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Carbon.Platform
{
    [DataContract]
    [StructLayout(LayoutKind.Explicit)]
    public struct ReportPeriod
    {
        public static readonly DateTime Epoch = new DateTime(2010, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        [FieldOffset(0)]
        private uint start; // In Seconds

        [FieldOffset(4)]
        private uint end; // In Seconds	

        [FieldOffset(0)]
        [DataMember(Order = 1)]
        public ulong Value;

        public DateTime Start => Epoch.AddSeconds(start);

        public DateTime End => Start.AddSeconds(end);

        public static ReportPeriod Create(ulong value)
        {
            return new ReportPeriod { Value = value };
        }

        public static ReportPeriod Create(DateTime start, DateTime end)
        {
            return new ReportPeriod {
                start = (uint)(start - Epoch).TotalSeconds,
                end   = (uint)(end - Epoch).TotalSeconds
            };
        }
    }
}