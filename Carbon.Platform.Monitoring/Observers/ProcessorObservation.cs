namespace Carbon.Platform
{
	using System;
	using System.Diagnostics;

	public class ProcessorObservation : IObservation
	{
		public CounterSample ProcessorTimeSample { get; set; }

		public DateTime Date { get; set; }
	}
}