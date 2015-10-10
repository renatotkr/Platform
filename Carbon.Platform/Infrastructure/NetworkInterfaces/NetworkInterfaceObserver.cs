namespace Carbon.Platform
{
	using System;
	using System.Diagnostics;

	public class NetworkInterfaceObserver : IDisposable
	{
		private readonly NetworkInterfaceInfo networkInterface;
		private readonly PerformanceCounter bytesReceivedPerSecondCounter;
		private readonly PerformanceCounter bytesSentPerSecondCounter;

		public NetworkInterfaceObserver(NetworkInterfaceInfo networkInterface)
		{
			#region Preconditions

			if (networkInterface == null) throw new ArgumentNullException("networkInterface");

			#endregion

			this.networkInterface = networkInterface;

			this.bytesReceivedPerSecondCounter	= new PerformanceCounter("Network Interface", "Bytes Received/sec"	, networkInterface.InstanceName);
			this.bytesSentPerSecondCounter		= new PerformanceCounter("Network Interface", "Bytes Sent/sec"		, networkInterface.InstanceName);
		}

		public NetworkInterfaceObservation Observe()
		{
			return new NetworkInterfaceObservation {
				NetworkInterface = networkInterface,
				ReceiveRateSample = bytesReceivedPerSecondCounter.NextSample(),
				SendRateSample = bytesSentPerSecondCounter.NextSample(),
				Date = DateTime.UtcNow
			};
		}

		public void Dispose()
		{
			bytesReceivedPerSecondCounter.Dispose();
			bytesSentPerSecondCounter.Dispose();
		}
	}
}
