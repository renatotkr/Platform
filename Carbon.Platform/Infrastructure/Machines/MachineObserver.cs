namespace Carbon.Platform
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	public class MachineObserver : IDisposable
	{
		private readonly ProcessorObserver processorObserver;
		private readonly NetworkInterfaceObserver[] networkInterfaceObservers;

		public MachineObserver(bool observeNetwork = false)
		{
			this.processorObserver = new ProcessorObserver("_Total");

			try
			{
				this.networkInterfaceObservers = GetNetworkInterfaceMonitors().ToArray();
			}
			catch { }
		}

		public MachineObservation Observe()
		{
			var machineObservation = new MachineObservation {
				Processor = processorObserver.Observe(),
				Date = DateTime.UtcNow
			};

			if (networkInterfaceObservers != null)
			{
				machineObservation.NetworkInterfaces = networkInterfaceObservers.Select(n => n.Observe()).ToArray();
			}

			return machineObservation;
		}

		public void Dispose()
		{
			processorObserver.Dispose();
		}



		public IEnumerable<NetworkInterfaceObserver> GetNetworkInterfaceMonitors()
		{
			
			foreach (var ni in Machine.GetActiveNetworkInterfaces())
			{
				var observer = ni.GetObserver();

				observer.Observe(); // Get an observation out of the way to make sure we don't throw later

				yield return observer;
			}

	
		}
	}
}