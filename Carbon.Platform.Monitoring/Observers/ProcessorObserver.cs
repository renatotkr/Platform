namespace Carbon.Platform
{
	using System;
	using System.Diagnostics;

	public class ProcessorObserver : IDisposable
	{
		private readonly PerformanceCounter processorTimeCounter;

		public ProcessorObserver(string processorName = "_Total")
		{
			#region Preconditions

			if (processorName == null) throw new ArgumentNullException("processorName");

			#endregion


			this.processorTimeCounter = new PerformanceCounter("Processor", "% Processor Time", processorName);
		}

		public ProcessorObservation Observe()
		{
			return new ProcessorObservation {
				ProcessorTimeSample = processorTimeCounter.NextSample(),
				Date = DateTime.UtcNow
			};
		}

		public void Dispose()
		{
			processorTimeCounter.Dispose();
		}
	}
}
