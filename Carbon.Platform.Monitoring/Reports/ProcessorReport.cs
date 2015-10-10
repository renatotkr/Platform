namespace Carbon.Platform
{
	using System;
	using System.Diagnostics;

	using Carbon;

	public class ProcessorReport
	{
        public DateTime Start { get; set; }

        public DateTime End { get; set; }
	
        /// <summary>
		/// The amount of time the processor was budy during the reporting period
		/// </summary>
		public float ProcessingTime { get; set; }

		public static ProcessorReport FromObservations(ProcessorObservation one, ProcessorObservation two)
		{
			float processingTime = CounterSample.Calculate(one.ProcessorTimeSample, two.ProcessorTimeSample);

			return new ProcessorReport 	{
                Start = one.Date,
                End = two.Date,
				ProcessingTime = (float)Math.Round(processingTime, 3)
			};
		}
	}
}