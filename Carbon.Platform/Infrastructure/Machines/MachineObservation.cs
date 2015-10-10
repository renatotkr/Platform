namespace Carbon.Platform
{
	using System;
	using System.Collections.Generic;

	public class MachineObservation : IObservation
	{
		public ProcessorObservation Processor { get; set; }

		public NetworkInterfaceObservation[] NetworkInterfaces { get; set; } 

		public DateTime Date { get; set; }
	}
}
