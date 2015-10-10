namespace Carbon.Platform
{
	using System;
	using System.Diagnostics;

	public class VolumeObservation : IObservation
	{
		public long Used { get; set; }
		
		public long Available { get; set; }
		
		public long Size { get; set; }

		public CounterSample ReadTimeSample { get; set; }

		public CounterSample WriteTimeSample { get; set; }
		
		public CounterSample ReadRateSample { get; set; }
	
		public CounterSample WriteRateSample { get; set; }

		public DateTime Date { get; set; }

		public VolumeInfo Volume { get; set; }
	}
}