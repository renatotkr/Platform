namespace Carbon.Platform
{
	using System;
	using System.Diagnostics;

	public class AppInstanceObservation
	{
		public AppInstance AppInstance { get; set; }

		public DateTime Date { get; set; }

		public CounterSample TotalRequestsSample { get; set; }

		public CounterSample SendRateSample { get; set; }

		public CounterSample ReceiveRateSample { get; set; }

		public CounterSample RequestRateSample { get; set; }

		public CounterSample ErrorRateSample { get; set; }

		public CounterSample UptimeSample { get; set; }
	}
}