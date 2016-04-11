using System;
using System.Runtime.InteropServices;

namespace Carbon.Platform
{
    [StructLayout(LayoutKind.Explicit)]
    public struct ReportIdentity
    {
        public static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        [FieldOffset(0)]
        private int duration; // In Seconds

        [FieldOffset(4)]
        private int timestamp; // In Seconds	

        [FieldOffset(0)]
        public long value;

        public long Value => value;

        public DateTime Start 
            => Epoch.AddSeconds(timestamp);

        public DateTime End 
            => Start.AddSeconds(duration);

        public static ReportIdentity Create(long value)
            => new ReportIdentity { value = value };

        public static long Create(DateTime start, DateTime end)
            => Create(start, end - start).Value;

        public static ReportIdentity Create(DateTime date, TimeSpan duration)
        {
            var time = (int)(date - Epoch).TotalSeconds;
            var d = (int)duration.TotalSeconds;

            return new ReportIdentity {
                timestamp = time,
                duration = d
            };
        }
    }
}
