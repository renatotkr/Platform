namespace Carbon.Platform
{
	using System;

	using Carbon;

	public class MemoryReport : IReport
	{
		public DateRange Period { get; set; }

		/// <summary>
		/// The number of bytes available at the end of the reporting period
		/// </summary>
		public long Available { get; set; }

		/// <summary>
		/// The number of commited bytes in use at the end of the reporting period
		/// </summary>
		public long Used { get; set; }

		/// <summary>
		/// The size (in bytes) of the installed ram
		/// </summary>
		public long Total { get; set; }
	}
}