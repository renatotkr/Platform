namespace Carbon.Platform
{
	using System;
	using System.Diagnostics;

	public class NetworkInterfaceObservation : IObservation
	{
		public NetworkInterfaceInfo NetworkInterface { get; set; }

		public CounterSample ReceiveRateSample { get; set; }

		public CounterSample SendRateSample { get; set; }

		public DateTime Date { get; set; }
	}
}