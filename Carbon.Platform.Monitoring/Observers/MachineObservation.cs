namespace Carbon.Platform
{
	using System;

	public class MachineObservation : IObservation
	{
        public int MachineId { get; set; }

		public ProcessorObservation Processor { get; set; }

		public NetworkInterfaceObservation[] NetworkInterfaces { get; set; } 

		public DateTime Date { get; set; }
	}
}
