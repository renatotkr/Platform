namespace Carbon.Platform
{
	using System;
	using System.Diagnostics;

	public class ProcessorReport
	{
        public DateTime Start { get; set; }

        public DateTime End { get; set; }
	
        /// <summary>
		/// The amount of time the processor was budy during the reporting period
		/// </summary>
		public float ProcessingTime { get; set; }
	}
}