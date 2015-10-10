namespace Carbon.Platform
{
	using System;
	using System.Runtime.InteropServices;

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

		public DateRange ToDateRange()
		{
			var start = Epoch.AddSeconds(timestamp);
			var d = TimeSpan.FromSeconds(duration);

			return new DateRange(start, d);
		}

		public static ReportIdentity Create(long value)
		{
			return new ReportIdentity {
				value = value
			};
		}

		public static ReportIdentity Create(DateRange range)
		{
			return Create(range.Start, range.TimeSpan);
		}

		public static ReportIdentity Create(DateTime date, TimeSpan duration)
		{
			var time = (int)(date - Epoch).TotalSeconds;
			var d	 = (int)duration.TotalSeconds;

			return new ReportIdentity {
				timestamp = time,
				duration = d
			};
		}
	}
}
