namespace Carbon.Platform
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	public class MachineObserver : IDisposable
	{
        private readonly int machineId;
		private readonly ProcessorObserver processorObserver;
		private readonly NetworkInterfaceObserver[] networkInterfaceObservers;

		public MachineObserver(int machineId, bool observeNetwork = false)
		{
            this.machineId = machineId;
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
                MachineId = machineId,
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
                var observer = new NetworkInterfaceObserver(ni);

				observer.Observe(); // Make an initial observersation to make sure things are OK

				yield return observer;
			}	
		}
	}
}