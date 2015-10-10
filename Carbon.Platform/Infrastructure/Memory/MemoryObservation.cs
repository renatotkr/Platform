namespace Carbon.Platform
{
	using System;

	public class MemoryObservation : IObservation
	{
		/// <summary>
		/// Each machine has a single pool of memory
		/// </summary>
		public long MachineId { get; set; }

		public long Available { get; set; }

		public long Used { get; set; }
		
		public long Total { get; set; }

		public DateTime Date { get; set; }
	}
}